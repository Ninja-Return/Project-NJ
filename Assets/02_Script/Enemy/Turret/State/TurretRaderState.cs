using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRaderState : TurretStateRoot
{
    private float delay;
    private float currentTime;

    public TurretRaderState(TurretFSM controller, float delay) : base(controller)
    {
        this.delay = delay;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (currentTime >= delay)
        {
            turretFSM.playerTrs = null;
            controller.ChangeState(TurretState.Fire);
        }
        else
        {
            currentTime += Time.deltaTime;

            Vector3 dir = (turretFSM.playerTrs.position - turretFSM.headTrs.position).normalized;
            turretFSM.headTrs.rotation = Quaternion.LookRotation(dir);
        }
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        currentTime = 0f;
    }
}
