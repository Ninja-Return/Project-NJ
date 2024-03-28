using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public class KillState : MonsterStateRoot
{
    public KillState(MonsterFSM controller) : base(controller)
    {

    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Attack", true);
        NetworkSoundManager.Play3DSound("MonsterDie", monsterFSM.transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);

        monsterFSM.JumpScare();

        nav.isStopped = true;
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (monsterFSM.IsKill) //1f => animTime
        {
            monsterFSM.IsKill = false;
            controller.ChangeState(MonsterState.Idle);
        }
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Attack", false);

        nav.isStopped = false;
    }
}
