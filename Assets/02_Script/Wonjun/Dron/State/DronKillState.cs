using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronKillState : DronStateRoot
{
    public DronKillState(DronFSM controller) : base(controller)
    {

    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        dronFSM.SetAnimation("Attack", true);
        NetworkSoundManager.Play3DSound("MonsterDie", dronFSM.transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);

        dronFSM.JumpScare();

        nav.isStopped = true;
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (dronFSM.IsKill) //1f => animTime
        {
            dronFSM.IsKill = false;
            controller.ChangeState(DronState.Idle);
        }
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        dronFSM.SetAnimation("Attack", false);

        nav.isStopped = false;
    }
}

