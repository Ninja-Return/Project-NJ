using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : InteractionObject
{

    [SerializeField] private Transform openingTrm;

    private bool isOpen;
    private bool isOpening;

    protected override void DoInteraction()
    {

        if (isOpening) return;

        isOpen = !isOpen;
        isOpening = true;

        if (isOpen)
        {

            SoundManager.Play3DSound("OpenDoor", transform.position, 1, 20);

            openingTrm.DOLocalMoveZ(0.03f, 0.3f)
                .SetEase(Ease.OutExpo)
                .OnComplete(() =>
                {

                    isOpening = false;

                });

        }
        else
        {

            SoundManager.Play3DSound("CloseDoor", transform.position, 1, 20);

            openingTrm.DOLocalMoveZ(-0.32f, 0.3f)
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

            interactionText = "E키를 눌러 서랍장을 닫기";

        }
        else
        {

            interactionText = "E키를 눌러 서랍장을 열기";

        }

    }

#if UNITY_EDITOR

    private void OnValidate()
    {

        gameObject.layer = 3;

    }

#endif
}
