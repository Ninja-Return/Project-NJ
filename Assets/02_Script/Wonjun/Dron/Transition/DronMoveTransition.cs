using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronMoveTransition : DronTransitionRoot
{
    public DronMoveTransition(DronFSM controller, DronState nextState) : base(controller, nextState) { }

    protected override bool CheckTransition()
    {
        if (nav.pathPending) return false;

        if (Vector3.Distance(nav.destination, controller.transform.position) <= 3f)
        {
            return true;
        }
        return false;
    }
}