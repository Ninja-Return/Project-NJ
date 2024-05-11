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
    private PlayerHand hand;

    private int originLayer;
    private bool isObjectMove;

    public PlayerInteraction(PlayerController controller) : base(controller)
    {

        interactionText = transform.Find("InteractionCanvas").Find("InteractionText").GetComponent<TMP_Text>();
        cameraTrm = transform.Find("PlayerCamera");
        interactionText.text = string.Empty;
        hand = GetComponent<PlayerHand>();

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

        if(interactionObject != null && interactionObject.interactionAble)
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


            var dest = Vector3.Distance(cameraTrm.position, info.transform.position);
            if (Physics.Raycast(cameraTrm.position, cameraTrm.forward, dest - 0.1f, ~data.InteractionLayer | ~LayerMask.GetMask("Player")))
            {

                interactionObject = null;
                interactionText.text = "";
                return;

            }

            interactionObject = info.transform.GetComponent<InteractionObject>();

            if (interactionObject != null && interactionObject.interactionAble)
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

                if(interactionObject == null)
                {

                    Debug.LogError("??????????");

                }


            }

        }
        else
        {

            interactionText.text = string.Empty;
            interactionObject = null;

        }

    }

}
