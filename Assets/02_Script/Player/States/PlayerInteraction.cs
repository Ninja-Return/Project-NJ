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
    private PlayerHand hand;

    private int originLayer;
    private bool isObjectMove;

    public PlayerInteraction(PlayerController controller) : base(controller)
    {

        interactionText = transform.Find("InteractionCanvas").Find("InteractionText").GetComponent<TMP_Text>();
        cameraTrm = transform.Find("PlayerCamera");
        moveObjectTrm = transform.Find("MoveObject");
        interactionText.text = string.Empty;
        hand = GetComponent<PlayerHand>();

    }

    protected override void EnterState()
    {

        input.OnInteractionKeyPress += HandleInteractionKeyPress;
        input.OnObjectMoveKeyUp += HandleObjectMoveKeyRelease;

    }



    protected override void ExitState()
    {

        input.OnInteractionKeyPress -= HandleInteractionKeyPress;
        input.OnObjectMoveKeyUp -= HandleObjectMoveKeyRelease;

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

            if(interactionObject.interactionAbleItemName == string.Empty)
            {

                interactionObject.Interaction();

            }
            else if(hand.CheckHandItem(interactionObject.interactionAbleItemName))
            {

                interactionObject.Interaction();

            }

        }

    }

    protected override void UpdateState()
    {

        CheckInteraction();

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

                if(interactionObject.interactionAbleItemName != string.Empty)
                {

                    if (hand.CheckHandItem(interactionObject.interactionAbleItemName))
                    {

                        interactionText.text = interactionObject.interactionText;

                    }
                    else
                    {

                        interactionText.text = $"{interactionObject.interactionAbleItemName}이(가) 필요합니다";

                    }

                }
                else
                {

                    interactionText.text = interactionObject.interactionText;

                }

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

}
