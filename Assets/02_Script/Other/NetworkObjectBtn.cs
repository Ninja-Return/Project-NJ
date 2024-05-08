using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NetworkObjectBtn : InteractionObject
{

    [SerializeField] private UnityEvent execute;
    [SerializeField] private bool isOnceInteraction;

    private string oldTex;

    private void Awake()
    {

        oldTex = interactionText;

    }

    private void Update()
    {

        if (!IsServer)
        {

            interactionText = "";
            interactionAble = false;

        }
        else
        {

            interactionText = oldTex;
            interactionAble = true;


        }

    }

    protected override void DoInteraction()
    {

        execute?.Invoke();

        if (isOnceInteraction)
        {

            gameObject.layer = 0;


        }

    }

}
