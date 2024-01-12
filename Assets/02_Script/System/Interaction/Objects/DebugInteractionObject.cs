using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInteractionObject : InteractionObject
{

    protected override void DoInteraction()
    {

        NetworkObject.Despawn(true);

    }

}
