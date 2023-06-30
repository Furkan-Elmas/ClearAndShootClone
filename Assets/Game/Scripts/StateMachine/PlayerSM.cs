using HyperlabCase.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase.StateMachine
{
    public class PlayerSM : BaseSM
    {
        private RunnerState runnerState;
        private CleaningState cleaningState;


        private void Awake()
        {
            runnerState = new RunnerState();
            cleaningState = new CleaningState();
        }

        private void OnEnable()
        {
            EventManager.OnPlayerTapToStart += BeginCleaning;
        }

        private void OnDisable()
        {
            EventManager.OnPlayerTapToStart -= BeginCleaning;
        }

        private void BeginCleaning()
        {
            ChangeState(cleaningState);
        }
    }
}
