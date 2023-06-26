using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSM : BaseSM
{
    private AttackState attackState;
    private CleaningState cleaningState;


    private void Awake()
    {
        attackState = new AttackState();
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
