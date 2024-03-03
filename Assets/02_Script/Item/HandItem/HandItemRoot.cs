using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class HandItemRoot : MonoBehaviour
{

    [ServerRpc(RequireOwnership = false)]
    public void UseServerRPC()
    {

        UseClientRPC();

    }

    [ClientRpc]
    private void UseClientRPC()
    {

        DoUse();

    }

    protected abstract void DoUse();

}
