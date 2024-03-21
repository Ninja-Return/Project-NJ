using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockObject : AreaUnlockObject
{

    [SerializeField] private List<InteractionObject> ableInter;

    protected override void DoInteraction()
    {

        foreach(var inter in ableInter)
        {

            inter.interactionAble = true;

        }

        base.DoInteraction();

    }

}
