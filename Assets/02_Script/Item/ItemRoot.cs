using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

//나중에 하위 카테고리 추가
public enum ItemCategory
{

    None = -2,
    Debug = -1,

}

[RequireComponent(typeof(NetworkTransform))]
public abstract class ItemRoot : InteractionObject
{

    [SerializeField] private SlotData data;

    public ItemCategory itemCategory { get; protected set; }

    protected override void DoInteraction()
    {

        Inventory.Instance.ObtainItem(data);
        Despawn();

    }

    public virtual void UseKeyPress() { }

    [ServerRpc(RequireOwnership = false)]
    public void SetSpawnServerRPC()
    {

        NetworkObject.Spawn(true);

    }
}
