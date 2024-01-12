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

    public PlayerInteraction(PlayerController controller) : base(controller)
    {

        interactionText = transform.Find("InteractionCanvas").GetComponentInChildren<TMP_Text>();
        cameraTrm = transform.Find("PlayerCamera");
        interactionText.text = string.Empty;

    }

    protected override void EnterState()
    {

        input.OnInteractionKeyPress += HandleInteractionKeyPress;

    }

    protected override void ExitState()
    {

        input.OnInteractionKeyPress -= HandleInteractionKeyPress;

    }

    private void HandleInteractionKeyPress()
    {

        if(interactionObject != null)
        {

            interactionObject.Interaction();

        }

    }

    protected override void UpdateState()
    {

        var hit = Physics.Raycast(cameraTrm.position, cameraTrm.forward, out var info, data.InteractionRange.Value, data.InteractionLayer);

        if(hit)
        {

            interactionObject = info.transform.GetComponent<InteractionObject>();

            if(interactionObject != null)
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

        }


    }

}
