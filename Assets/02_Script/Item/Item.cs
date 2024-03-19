using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Item : ItemRoot
{

    private void Start()
    {

        interactionText = $"E�� ���� {data.itemName}�� ����";

    }
#if UNITY_EDITOR

    private void OnValidate()
    {

        gameObject.layer = 3;

    }

#endif


}
