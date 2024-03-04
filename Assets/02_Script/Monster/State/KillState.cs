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
        //if (!IsServer) return;
        //animTime = anim.GetCurrentAnimatorStateInfo(0).length; 애니메이션 되면 씁시다.
    }

    protected override void UpdateState()
    {
        //if (!IsServer) return;

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
        currentTime = 0f;
    }
}
