using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculpturePingTransition : SculptureTransitionRoot
{
    public SculpturePingTransition(SculptureFSM controller, SculptureState nextState) : base(controller, nextState) { }

    protected override bool CheckTransition()
    {
        if (sculptureFSM.IsPing)
        {
            return true;
        }
        return false;
    }
}
