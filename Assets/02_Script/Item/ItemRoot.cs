using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//나중에 하위 카테고리 추가
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
