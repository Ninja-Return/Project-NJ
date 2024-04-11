using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronPingState : DronStateRoot
{
    private float speed;

    public DronPingState(DronFSM controller, float speed) : base(controller)
    {
        this.speed = speed;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        dronFSM.SetAnimation("Work", true);
        NetworkSoundManager.Play3DSound("DronPing", dronFSM.transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);

        nav.speed = speed;

        Vector3 pos = dronFSM.pingPos;
        nav.SetDestination(pos);
    }

    protected override void UpdateState()
    {
        base.UpdateState();
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        dronFSM.SetAnimation("Work", false);

        nav.SetDestination(dronFSM.transform.position);
    }
}
