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

    private HandItemRoot currentObject;
    private PlayerAnimationController controller;
    private PlayerController playerController;
    private int currentIdx = -1;

    private IEnumerator Start()
    {

        yield return null;

        controller = GetComponent<PlayerAnimationController>();
        itemParent.transform.position = controller.GetHandTarget().position;

        if (IsOwner)
        {

            Inventory.Instance.OnSlotClickEvt += HandleHold;
            Inventory.Instance.OnSlotDropEvt += HandleDrop;

            playerController = GetComponent<PlayerController>();
            playerController.Input.OnUseObjectKeyPress += HandleHandUse;

        }

    }

    private void HandleDrop(string objKey, int idx)
    {

        if(currentIdx == idx)
        {

            HandDeleteServerRPC();
            controller.HandControl(false);

        }

    }

    private void HandleHandUse()
    {

        if(currentObject != null)
        {

            UseServerRPC();

        }


    }

    [ServerRpc]
    private void HandDeleteServerRPC()
    {

        HandDeleteClientRPC();

    }

    [ClientRpc]
    private void HandDeleteClientRPC()
    {

        Destroy(currentObject.gameObject);
        currentObject = null;
        currentIdx = -1;

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

        currentIdx = idx;

    }

    [ServerRpc]
    private void SpawnItemServerRPC(FixedString128Bytes objKey)
    {

        SpawnItemClientRPC(objKey);

    }

    [ClientRpc]
    private void SpawnItemClientRPC(FixedString128Bytes objKey)
    {

        var obj = Resources.Load<HandItemRoot>($"ItemObj/{objKey}_Hand");

        currentObject = Instantiate(obj, itemParent);
        currentObject.transform.localPosition = Vector3.zero + currentObject.handPivot;
        currentObject.transform.localEulerAngles = currentObject.handRotation;

    }

    [ServerRpc]
    private void UseServerRPC()
    {

        UseClientRPC();

    }

    [ClientRpc]
    private void UseClientRPC()
    {

        currentObject.DoUse();

    }

    public bool CheckHandItem(string targetName)
    {

        return Inventory.Instance.GetItemName(currentIdx) == targetName;

    }

    public void UseHandItem()
    {

        Inventory.Instance.Deleteltem();
        controller.HandControl(false);
        UseHandServerRPC();

    }

    [ServerRpc]
    private void UseHandServerRPC()
    {

        HandDeleteClientRPC();

    }

}
