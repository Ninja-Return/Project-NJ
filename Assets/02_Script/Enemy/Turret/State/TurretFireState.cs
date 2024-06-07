using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFireState : TurretStateRoot
{
    public TurretFireState(TurretFSM controller) : base(controller)
    {
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        controller.ChangeState(TurretState.Idle);
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

    }
}
