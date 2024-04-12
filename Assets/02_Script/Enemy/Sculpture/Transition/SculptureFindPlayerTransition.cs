using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculptureFindPlayerTransition : SculptureTransitionRoot
{
    private float radius;

    public SculptureFindPlayerTransition(SculptureFSM controller, SculptureState nextState, float radius) : base(controller, nextState) 
    {
        this.radius = radius;
    }

    protected override bool CheckTransition()
    {
        if (nav.pathPending) return false;

        Collider targetPlayer = sculptureFSM.CirclePlayer(radius);
        if (targetPlayer != null)
        {
            sculptureFSM.targetPlayer = targetPlayer;
            return true;
        }

        return false;
    }
}
