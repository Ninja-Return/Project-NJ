using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CraftingButton : InteractionObject
{

    [SerializeField] private CraftingTable table;

    protected override void DoInteraction()
    {

        CraftItemServerRPC();

    }

    [ServerRpc(RequireOwnership = false)]
    private void CraftItemServerRPC()
    {

        table.CraftingItem();

    }

}