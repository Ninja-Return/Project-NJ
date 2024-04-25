using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

//나중에 하위 카테고리 추가
public enum ItemCategory
{

    None = -2,
    Debug = -1,

}
public abstract class ItemRoot : InteractionObject
{

    [field:SerializeField] public ItemDataSO data { get; private set; }

    public string extraData { get; set; }
    public ItemCategory itemCategory { get; protected set; }

    public event Action DestroyCallback;

    protected override void DoInteraction()
    {

        if (Inventory.Instance.ObtainItem(data, extraData))
        {

            Despawn();

        }

    }

    protected override void OnDespawn()
    {

        DestroyCallback?.Invoke();

    }

    [ServerRpc(RequireOwnership = false)]
    public void SetUpExtraDataServerRPC(FixedString32Bytes str)
    {

        SetUpExtraDataClientRPC(str);

    }

    [ClientRpc]
    private void SetUpExtraDataClientRPC(FixedString32Bytes str)
    {

        SetUpExtraData(str.ToString());

    }

    protected virtual void SetUpExtraData(string str) { extraData = str; }


}
