using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperlabCase.Interfaces;

namespace HyperlabCase.StateMachine
{
    public class BaseSM : MonoBehaviour
    {
        private IState currentState;
        private IState previousState;

        private bool inTransition;


        private void Update()
        {
            if (currentState != null && !inTransition)
                currentState.Tick();
        }

        public void ChangeState(IState newState)
        {
            if (currentState == newState || inTransition)
                return;

            StartCoroutine(ChangeStateCoroutine(newState));
        }

        private IEnumerator ChangeStateCoroutine(IState newState)
        {
            inTransition = true;

            if (currentState != null)
                currentState.Exit();

            if (previousState != null)
                previousState = currentState;

            currentState = newState;

            yield return null;

            if (currentState != null)
                currentState.Enter();

            inTransition = false;
        }
    } 
}
