using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerHand : NetworkBehaviour
{

    private Transform itemParent;
    private HandItemRoot currentObject;
    private PlayerAnimationController controller;
    private PlayerController playerController;
    private int currentIdx = -1;
    private bool isDelay;

    private IEnumerator Start()
    {

        yield return null;

        controller = GetComponent<PlayerAnimationController>();
        itemParent = controller.GetHandTarget();

        if (IsOwner)
        {

            Inventory.Instance.OnSlotClickEvt += HandleHold;
            Inventory.Instance.OnSlotDropEvt += HandleDrop;
            Inventory.Instance.OnSlotRemove += HandleDrop;
            playerController = GetComponent<PlayerController>();
            playerController.Input.OnUseObjectKeyPress += HandleHandUse;
            playerController.Input.OnUseObjectKeyUp += HandleHandUp;

        }

    }

    private void HandleHandUp()
    {
        if (isDelay) return;
        if (Inventory.Instance.showingDelay || Inventory.Instance.isShow) return;

        if (currentObject != null)
        {

            if (currentObject.isLocalUse)
            {

                currentObject.DoRelease();

            }
            else
            {

                HandUpServerRPC();

            }

        }

    }

    private void HandleDrop(string objKey, int idx, string extraData)
    {

        if(currentIdx == idx)
        {

            HandDeleteServerRPC();
            controller.HandControl(false);

        }

    }

    private void HandleHandUse()
    {

        if (isDelay) return;
        if (Inventory.Instance.showingDelay || Inventory.Instance.isShow) return;

        if (currentObject != null)
        {

            if (currentObject.isLocalUse)
            {

                currentObject.DoUse();

            }
            else
            {

                UseServerRPC();

            }

            if (currentObject.is1Use)
            {

                Destroy(currentObject.gameObject);

                Inventory.Instance.Deleteltem();

                HandDeleteServerRPC();
                controller.HandControl(false);

            }

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

        if (currentObject == null) return;

        Destroy(currentObject.gameObject);
        currentObject = null;
        currentIdx = -1;

    }

    [ServerRpc]
    private void HandUpServerRPC()
    {

        HandUpClientRPC();

    }

    [ClientRpc]
    private void HandUpClientRPC()
    {

        if (currentObject == null) return;
        
        currentObject.DoRelease();

    }


    private void HandleHold(string objKey, int idx, string extraData)
    {
        if (isDelay) return;

        StartCoroutine(DelayCo());
        Inventory.Instance.SetActiveInventoryUI();


        if (currentIdx == idx && currentObject != null)
        {

            DeleteServerRPC();
            controller.HandControl(false);
            return;

        }

        if(currentObject != null)
        {

            DeleteServerRPC(true);

        }

        SpawnItemServerRPC(objKey, OwnerClientId);

        controller.HandControl(true);

        currentIdx = idx;

        controller.InitHandTarget();

    }

    [ServerRpc]
    private void SpawnItemServerRPC(FixedString128Bytes objKey, ulong ownerId)
    {

        SpawnItemClientRPC(objKey, ownerId);

    }

    [ClientRpc]
    private void SpawnItemClientRPC(FixedString128Bytes objKey, ulong ownerId)
    {

        var obj = Resources.Load<HandItemRoot>($"ItemObj/{objKey}_Hand");


        currentObject = Instantiate(obj, itemParent);
        currentObject.Spawn(NetworkManager.LocalClientId == ownerId);
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

        if (currentObject == null) return;

        currentObject.DoUse();

    }

    public bool CheckHandItem(string targetName)
    {

        return Inventory.Instance.GetItemName(currentIdx) == targetName;

    }


    [ServerRpc]
    private void DeleteServerRPC(bool isChange = false)
    {

        DeleteClientRPC(isChange);

    }

    [ClientRpc]
    private void DeleteClientRPC(bool isChange)
    {

        if(currentObject != null)
        {

            Destroy(currentObject.gameObject);
            currentObject = null;

            if (!isChange)
            {

                currentIdx = -1;

            }


        }

    }

    private IEnumerator DelayCo()
    {

        isDelay = true;

        yield return new WaitForSeconds(0.2f);

        isDelay = false;

    }

}
