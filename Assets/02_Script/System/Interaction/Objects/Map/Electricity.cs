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
            interactionText = "EŰ�� ���� ������ �����ϼ���";

        }
        else
        {

            interactionAble = false;
            interactionText = "";

        }

    }

}