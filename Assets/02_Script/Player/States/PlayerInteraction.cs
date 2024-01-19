using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteraction : PlayerStateRoot
{

    private TMP_Text interactionText;
    private Transform cameraTrm;
    private InteractionObject interactionObject;
    private Transform moveObjectTrm;

    private int originLayer;
    private bool isObjectMove;

    public PlayerInteraction(PlayerController controller) : base(controller)
    {

        interactionText = transform.Find("InteractionCanvas").GetComponentInChildren<TMP_Text>();
        cameraTrm = transform.Find("PlayerCamera");
        moveObjectTrm = transform.Find("MoveObject");
        interactionText.text = string.Empty;

    }

    protected override void EnterState()
    {

        input.OnInteractionKeyPress += HandleInteractionKeyPress;
        input.OnObjectMoveKeyPress += HandleObjectMoveKeyPress;
        input.OnObjectMoveKeyUp += HandleObjectMoveKeyRelease;

    }



    protected override void ExitState()
    {

        input.OnInteractionKeyPress -= HandleInteractionKeyPress;
        input.OnObjectMoveKeyPress -= HandleObjectMoveKeyPress;
        input.OnObjectMoveKeyUp -= HandleObjectMoveKeyRelease;

    }

    private void HandleObjectMoveKeyPress()
    {

        if(interactionObject != null && interactionObject.objectType == ObjectType.Dynamic) 
        { 
            
            isObjectMove = true;
            originLayer = interactionObject.gameObject.layer;
            interactionObject.gameObject.layer = LayerMask.NameToLayer("NotCasting");
            interactionText.text = string.Empty;
        
        }

    }

    private void HandleObjectMoveKeyRelease()
    {

        isObjectMove = false;

        if(interactionObject != null)
        {

            interactionObject.gameObject.layer = originLayer;

        }

    }


    private void HandleInteractionKeyPress()
    {

        if(interactionObject != null && !isObjectMove)
        {

            interactionObject.Interaction();

        }

    }

    protected override void UpdateState()
    {

        CheckInteraction();
        SetMoveObjectPos();
        MovementObject();

    }

    private void MovementObject()
    {

        if (isObjectMove)
        {

            interactionObject.transform.position = moveObjectTrm.position;

        }

    }

    private void CheckInteraction()
    {

        if (isObjectMove) return;

        var hit = Physics.Raycast(cameraTrm.position, cameraTrm.forward, out var info, data.InteractionRange.Value, data.InteractionLayer);

        if (hit)
        {

            interactionObject = info.transform.GetComponent<InteractionObject>();

            if (interactionObject != null)
            {

                interactionText.text = interactionObject.interactionText;

            }
            else
            {

                interactionText.text = string.Empty;

            }

        }
        else
        {

            interactionText.text = string.Empty;
            interactionObject = null;

        }

    }

    private void SetMoveObjectPos()
    {

        var hit = Physics.Raycast(cameraTrm.position, cameraTrm.forward, out var info, data.InteractionRange.Value, ~LayerMask.GetMask("NotCasting", "Player"));

        if (hit)
        {

            moveObjectTrm.position = info.point - cameraTrm.forward / 1.5f;

        }
        else
        {

            moveObjectTrm.position = cameraTrm.position + cameraTrm.forward * (data.InteractionRange.Value / 1.5f);

        }

    }

}
