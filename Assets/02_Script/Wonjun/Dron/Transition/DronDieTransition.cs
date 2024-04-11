using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronDieTransition : DronTransitionRoot
{
    public DronDieTransition(DronFSM controller, DronState nextState) : base(controller, nextState) { }

    protected override bool CheckTransition()
    {
        if (dronFSM.IsDead)
        {
            return true;
        }
        return false;
    }
}
