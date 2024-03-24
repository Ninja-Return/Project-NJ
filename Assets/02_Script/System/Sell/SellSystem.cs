using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class SellSystem : NetworkBehaviour
{

    [SerializeField] private Transform sellAreaTrm;
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private LayerMask itemLayer;

    public void Sell(ulong sellId)
    {

        SellServerRPC(sellId);

    }

    [ServerRpc(RequireOwnership = false)]
    private void SellServerRPC(ulong sellId)
    {

        var objs = Physics.OverlapBox(sellAreaTrm.position , boxSize / 2, Quaternion.identity, itemLayer);

        int price = 0;

        foreach(var item in objs)
        {

            if(item.TryGetComponent<ItemRoot>(out var item_r))
            {

                price += item_r.data.price;
                item_r.NetworkObject.Despawn();

            }

        }

        SellClientRPC(price, sellId.GetRPCParams());

    }

    [ClientRpc]
    private void SellClientRPC(int price,ClientRpcParams param)
    {

        Debug.Log(2);


        var player = FindObjectsOfType<PlayerController>().ToList()
            .Find(x => x.OwnerClientId == NetworkManager.LocalClientId);

        player.GetComponent<CreditSystem>().Credit += price;

    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {

        if (sellAreaTrm == null) return;

        Gizmos.DrawWireCube(sellAreaTrm.position, boxSize);

    }

#endif

}
