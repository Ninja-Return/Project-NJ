using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerHand : NetworkBehaviour
{

    private GameObject currentObject;
    private PlayerAnimationController controller;
    private Transform itemParent;
    private int currentIdx = -1;

    private IEnumerator Start()
    {

        yield return null;

        if (!IsOwner) yield break;

        controller = GetComponent<PlayerAnimationController>();
        itemParent = controller.GetHandTarget();

    }

    private void HandleClickItem(GameObject obj, int index)
    {

        if(currentIdx == index && currentObject != null)
        {

            Destroy(currentObject);
            currentIdx = -1;

            controller.HandControl(false);

            return;

        }

        if(currentObject != null)
        {

            Destroy(currentObject);

        }

        var cloneObj = Instantiate(obj, itemParent).GetComponent<NetworkObject>();
        cloneObj.Spawn(true);

        currentIdx = index;

        controller.HandControl(true);

    }

}
