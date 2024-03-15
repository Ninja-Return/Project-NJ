using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;
using FSM_System;

public class IdleState : MonsterStateRoot
{
    private float idleDuration = 3f;
    private float currentTime = 0;

    public IdleState(MonsterFSM controller) : base(controller) { }

    protected override void EnterState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Idle", true);
        NetworkSoundManager.Play3DSound("MonsterHowling", monsterFSM.transform.position, 0.1f, 30f);
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

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
        if (!IsServer) return;

        monsterFSM.SetAnimation("Idle", false);
        currentTime = 0;
    }
}
