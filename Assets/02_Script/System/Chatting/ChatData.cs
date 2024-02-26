using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct ChatData : INetworkSerializable, IEquatable<ChatData>
{

    public FixedString512Bytes message;
    public FixedString64Bytes userName;
    public ulong clientId;
    public int messageId; //�޽����� ������ ǥ���ϴ� ��

    public ChatData(string message, ulong clientId)
    {

        this.message = message;
        this.clientId = clientId;
        messageId = 0;
        userName = "";

    }

    public bool Equals(ChatData other)
    {

        return messageId == other.messageId;

    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {

        serializer.SerializeValue(ref message);
        serializer.SerializeValue(ref userName);
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref messageId);

    }

}
