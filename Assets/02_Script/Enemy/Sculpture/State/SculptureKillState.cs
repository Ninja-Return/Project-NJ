using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculptureKillState : SculptureStateRoot
{
    public SculptureKillState(SculptureFSM controller) : base(controller)
    {
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        NetworkSoundManager.Play3DSound("BoneChuck", sculptureFSM.transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);

        sculptureFSM.KillPlayer();

        nav.isStopped = true;
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (sculptureFSM.IsKill) //1f => animTime
        {
            sculptureFSM.IsKill = false;
            controller.ChangeState(SculptureState.Patrol);
        }
    }

    protected override void ExitState() { }
}
