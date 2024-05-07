using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : InteractionObject
{
    protected override void DoInteraction()
    {

        MapLightSystem.Instance.EnableLight();

    }

    private void Update()
    {

        if(New_GameManager.Instance == null) return;

        if (!New_GameManager.Instance.IsLightOff.Value)
        {

            interactionAble = true;
            interactionText = "E키를 눌러 전력을 복구하세요";

        }
        else
        {

            interactionAble = false;
            interactionText = "";

        }

    }

}