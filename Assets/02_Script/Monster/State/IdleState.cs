using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;
using FSM_System;

public class IdleState : MonsterStateRoot
{
    private float idleDuration = 1f;
    private float currentTime = 0;

    public IdleState(MonsterFSM controller) : base(controller) { }

    protected override void EnterState()
    {
        
    }

    protected override void UpdateState()
    {
        //if (!IsServer) return;

        if (currentTime >= idleDuration)
        {
            controller.ChangeState(MonsterState.Patrol);
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

    protected override void ExitState()
    {
        currentTime = 0;
    }
}
