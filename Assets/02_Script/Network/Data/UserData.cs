using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[System.Serializable]
public struct UserData : INetworkSerializable
{

    public string nickName;
    public string authId;
    public List<AttachedItem> attachedItem;
    public bool isDie;
    public bool isBreak;
    public float clearTime;
    public Color color;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref nickName);
        serializer.SerializeValue(ref authId);
        //serializer.SerializeValue(ref attachedItem);
        serializer.SerializeValue(ref isDie);
        serializer.SerializeValue(ref isBreak);
        serializer.SerializeValue(ref clearTime);
        serializer.SerializeValue(ref color);
    }
}