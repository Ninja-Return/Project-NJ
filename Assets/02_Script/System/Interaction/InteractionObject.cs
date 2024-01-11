using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class InteractionObject : NetworkBehaviour
{

    [field:SerializeField] public string interactionText { get; protected set; }

    protected abstract void DoInteraction();

    public void Interaction()
    {

        InteractionServerRPC();

    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractionServerRPC()
    {

        InteractionClientRPC();

    }

    [ClientRpc]
    private void InteractionClientRPC()
    {

        DoInteraction();

    }

}
