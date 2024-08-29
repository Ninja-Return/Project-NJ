using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : PlayerStateRoot
{

    private TMP_Text interactionText;
    private Image itemCircle;
    private Transform cameraTrm;
    private InteractionObject interactionObject;
    private PlayerHand hand;

    private int originLayer;
    private bool isObjectMove;

    private Coroutine circleCor;

    public PlayerInteraction(PlayerController controller) : base(controller)
    {

        interactionText = transform.Find("InteractionCanvas").Find("InteractionText").GetComponent<TMP_Text>();
        itemCircle = transform.Find("InteractionCanvas").Find("ItemCircle").GetComponent<Image>();
        cameraTrm = transform.Find("PlayerCamera");
        interactionText.text = string.Empty;
        hand = GetComponent<PlayerHand>();

    }

    protected override void EnterState()
    {

        input.OnInteractionKeyDown += HandleInteractionKeyDown;
        input.OnInteractionKeyUp += HandleInteractionKeyUp;

    }



    protected override void ExitState()
    {

        input.OnInteractionKeyDown -= HandleInteractionKeyDown;
        input.OnInteractionKeyUp -= HandleInteractionKeyUp;

        circleCor = null;

    }


    private void HandleInteractionKeyDown()
    {

        circleCor = StartCoroutine(GetInteractionCor());

    }

    private void HandleInteractionKeyUp()
    {
        if (circleCor != null)
        {
            StopCoroutine(circleCor);
            itemCircle.fillAmount = 0f;
        }
    }

    private bool CheckItemCircle()
    {
        itemCircle.fillAmount += Time.deltaTime * 3f;

        if (itemCircle.fillAmount >= 1f)
        {
            itemCircle.fillAmount = 0f;
            return true;
        }
        return false;
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


            var dest = Vector3.Distance(cameraTrm.position, info.point);
            if (Physics.Raycast(cameraTrm.position, cameraTrm.forward, dest - 0.3f, ~data.InteractionLayer | ~LayerMask.GetMask("Player")))
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

    private IEnumerator GetInteractionCor()
    {
        while (true)
        {
            if (interactionObject != null && interactionObject.interactionAble)
            {
                if (interactionObject.interactionAbleItemName == string.Empty || hand.CheckHandItem(interactionObject.interactionAbleItemName))
                {
                    if (!CheckItemCircle())
                    {
                        yield return new WaitForSeconds(0.01f);
                        continue;
                    }
                    interactionObject.Interaction();
                    break;

                }
            }
            break;
        }
        itemCircle.fillAmount = 0f;

        yield return null;
    }

}
