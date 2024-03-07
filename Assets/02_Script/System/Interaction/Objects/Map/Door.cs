using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractionObject
{

    private bool isOpen;
    private bool isOpening;

    protected override void DoInteraction()
    {

        if (isOpening) return;

        isOpen = !isOpen;
        isOpening = true;

        if (isOpen)
        {

            transform.DOLocalRotate(new Vector3(0, 130, 0), 0.3f)
                .SetEase(Ease.OutExpo)
                .OnComplete(() =>
                {

                    isOpening = false;

                });

        }
        else
        {

            transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f)
                .SetEase(Ease.OutExpo)
                .OnComplete(() =>
                {
                
                    isOpening = false;
                
                });

        }

    }

    private void Update()
    {

        if (isOpening) return;

        if (isOpen)
        {

            interactionText = "E키를 눌러 문을 닫기";

        }
        else
        {

            interactionText = "E키를 눌러 문을 열기";

        }

    }

}
