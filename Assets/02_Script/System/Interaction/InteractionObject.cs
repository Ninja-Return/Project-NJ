using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum ObjectType
{

    Dynamic,
    Static

}

public abstract class InteractionObject : NetworkBehaviour
{

    [field:SerializeField] public string interactionText { get; protected set; }
    [field:SerializeField] public ObjectType objectType { get; protected set; }
    [field:SerializeField] public bool rpcOfLocalClient { get; protected set; }
    [field:SerializeField] public string interactionAbleItemName {  get; protected set; }

    public bool interactionAble = true;

    protected abstract void DoInteraction();

    public virtual void Interaction()
    {

        if (rpcOfLocalClient)
        {

            DoInteraction();

        }
        else
        {

            InteractionServerRPC(NetworkManager.LocalClientId);

        }

    }


    [ServerRpc(RequireOwnership = false)]
    protected virtual void InteractionServerRPC(ulong localClient)
    {

        InteractionClientRPC();

    }

    [ClientRpc]
    protected virtual void InteractionClientRPC(ClientRpcParams param = default)
    {

        DoInteraction();

    }

    public void Despawn()
    {

        DespawnServerRPC();

    }

    [ServerRpc(RequireOwnership = false)]
    private void DespawnServerRPC()
    {

        OnDespawn();
        NetworkObject.Despawn(true);

    }

    protected virtual void OnDespawn() { }

}
