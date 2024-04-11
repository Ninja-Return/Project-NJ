using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronIdleState : DronStateRoot
{
    private float idleDuration = 3f;
    private float currentTime = 0;

    public DronIdleState(DronFSM controller) : base(controller) { }

    protected override void EnterState()
    {
        if (!IsServer) return;

        dronFSM.SetAnimation("Idle", true);
        NetworkSoundManager.Play3DSound("DronHowling", dronFSM.transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (currentTime >= idleDuration)
        {
            controller.ChangeState(DronState.Patrol);
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        dronFSM.SetAnimation("Idle", false);
        currentTime = 0;
    }
}
