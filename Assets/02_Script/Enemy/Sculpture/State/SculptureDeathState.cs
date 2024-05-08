using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SculptureDeathState : SculptureStateRoot
{
    public SculptureDeathState(SculptureFSM controller) : base(controller)
    {
        
    }

    protected override void EnterState()
    {
        sculptureFSM.CreateDeadbody();
        sculptureFSM.NetworkObject.Despawn();
    }

    protected override void UpdateState()
    {

    }

    protected override void ExitState()
    {
        
    }
}
