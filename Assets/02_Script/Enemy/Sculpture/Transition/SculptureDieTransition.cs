using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculptureDieTransition : SculptureTransitionRoot
{
    public SculptureDieTransition(SculptureFSM controller, SculptureState nextState) : base(controller, nextState)
    {
    }

    protected override bool CheckTransition()
    {
        if (sculptureFSM.IsDead)
        {
            return true;
        }
        return false;
    }
}
