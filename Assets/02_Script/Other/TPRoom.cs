using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPRoom : InteractionObject
{

    [SerializeField] private Vector3 tpPos;

    protected override void DoInteraction()
    {

        PlayerManager.Instance.localController.transform.position = tpPos;

    }

}
