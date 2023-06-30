using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase.StateMachine
{
    public class WeaponSM : MonoBehaviour
    {
        private bool canShoot;


        private void Update()
        {
            if (!canShoot)
                return;


        }

        public void StartShooting()
        {

        }
    } 
}
