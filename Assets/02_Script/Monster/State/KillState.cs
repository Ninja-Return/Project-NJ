using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public class KillState : MonsterStateRoot
{
    private float animTime;
    private float currentTime = 0;

    public KillState(MonsterFSM controller) : base(controller)
    {

    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Attack", true);

        nav.isStopped = true;
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (currentTime >= 1f) //1f => animTime
        {
            controller.ChangeState(MonsterState.Idle);
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Attack", false);

        nav.isStopped = false;

        currentTime = 0f;
    }
}
