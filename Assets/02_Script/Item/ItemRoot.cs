using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���߿� ���� ī�װ� �߰�
public enum ItemCategory
{

    None = -2,
    Debug = -1,

}

[RequireComponent(typeof(ClientNetworkTransform))]
public abstract class ItemRoot : InteractionObject
{
    
    public ItemCategory itemCategory { get; protected set; }

}
