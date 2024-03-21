using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractionObject
{

    private bool isOpen;
    private bool isOpening;
    private float origin;

    private void Start()
    {
        
        origin = transform.localEulerAngles.y;

    }

    protected override void DoInteraction()
    {

        if (isOpening) return;

        isOpen = !isOpen;
        isOpening = true;



        if (isOpen)
        {

            SoundManager.Play3DSound("OpenDoor", transform.position, 1, 20);

            transform.DOLocalRotate(new Vector3(0, origin + 90, 0), 0.3f)
                .SetEase(Ease.OutExpo)
                .OnComplete(() =>
                {

                    isOpening = false;

                });

        }
        else
        {

            SoundManager.Play3DSound("CloseDoor", transform.position, 1, 20);

            transform.DOLocalRotate(new Vector3(0, origin, 0), 0.3f)
                .SetEase(Ease.OutExpo)
                .OnComplete(() =>
                {
                
                    isOpening = false;
                
                });

        }

    }

    public bool IsDoorOpenning() { return isOpening; }

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

#if UNITY_EDITOR

    private void OnValidate()
    {

        gameObject.layer = 3;

    }

#endif

}
