using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using WebSocketSharp;

public class PlayerHand : NetworkBehaviour
{

    [SerializeField] private Transform itemParent;

    private GameObject currentObject;
    private PlayerAnimationController controller;
    private int currentIdx = -1;

    private IEnumerator Start()
    {

        yield return null;

        controller = GetComponent<PlayerAnimationController>();
        itemParent.transform.position = controller.GetHandTarget().position;

        if (IsOwner)
        {

            Inventory.Instance.OnSlotClickEvt += HandleHold;

        }

    }

    private void HandleHold(string objKey, int idx)
    {
        
        if(currentIdx == idx && currentObject != null)
        {

            Destroy(currentObject);
            currentObject = null;
            controller.HandControl(false);
            return;

        }

        if(currentObject != null)
        {

            Destroy(currentObject);

        }

        SpawnItemServerRPC(objKey);

        controller.HandControl(true);

    }

    [ServerRpc]
    private void SpawnItemServerRPC(FixedString128Bytes objKey)
    {

        SpawnItemClientRPC(objKey);

    }

    [ClientRpc]
    private void SpawnItemClientRPC(FixedString128Bytes objKey)
    {

        var obj = Resources.Load<GameObject>($"ItemObj/{objKey}_Hand");

        currentObject = Instantiate(obj, itemParent);
        currentObject.transform.localPosition = Vector3.zero;

    }

}
