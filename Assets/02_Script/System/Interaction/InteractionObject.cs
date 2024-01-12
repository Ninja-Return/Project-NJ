using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class InteractionObject : NetworkBehaviour
{

    [field:SerializeField] public string interactionText { get; protected set; }

    protected abstract void DoInteraction();

    public virtual void Interaction()
    {

        InteractionServerRPC();

    }

    [ServerRpc(RequireOwnership = false)]
    protected virtual void InteractionServerRPC()
    {

        InteractionClientRPC();

    }

    [ClientRpc]
    protected virtual void InteractionClientRPC()
    {

        DoInteraction();

    }

}
