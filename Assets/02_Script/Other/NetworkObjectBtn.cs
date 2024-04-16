using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NetworkObjectBtn : InteractionObject
{

    [SerializeField] private UnityEvent execute;

    private void Update()
    {

        if (!IsHost)
        {

            interactionText = "";
            interactionAble = false;

        }

    }

    protected override void DoInteraction()
    {

        execute?.Invoke();

    }

}
