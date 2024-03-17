using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UserData
{

    public string nickName;
    public string authId;
    public List<AttachedItem> attachedItem;
    public bool isDie;
    public Color color;

}