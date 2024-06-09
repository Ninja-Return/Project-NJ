using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrapperDieState : TrapperStateRoot
{
    public TrapperDieState(TrapperFSM controller) : base(controller)
    {
        
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        trapperFSM.nav.isStopped = true;
        //trapperFSM.SetAnimation("IsDeath");

        trapperFSM.gameObject.layer = 0;
    }

    protected override void UpdateState()
    {
        
    }

    protected override void ExitState()
    {

    }
}
