using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronDeathState : DronStateRoot
{
    public DronDeathState(DronFSM controller) : base(controller)
    {

    }

    protected override void EnterState()
    {
        if (!IsServer) return;


        //죽는 애니메이션
        //리썰처럼 시체 냅두는게 좋을듯?
    }

    protected override void UpdateState()
    {

    }

    protected override void ExitState()
    {
        if (!IsServer) return;

    }
}
