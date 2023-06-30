using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase.Interfaces
{
    public interface IState
    {
        public void Enter();
        public void Tick();
        public void Exit();
    } 
}
