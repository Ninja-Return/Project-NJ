using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public delegate void OnChatting(ChatData chatData);

public class ChattingSystem : NetworkBehaviour
{

    private readonly int MAX_CHAT_CHAR = 100;

    private NetworkList<ChatData> chatList;
    public event OnChatting OnChattingAdd;

    private void Awake()
    {

        chatList = new();

    }

    private void Start()
    {

        if (IsClient)
        {

            chatList.OnListChanged += HandleChattingChanged;

        }


    }

    private void HandleChattingChanged(NetworkListEvent<ChatData> changeEvent)
    {

        switch (changeEvent.Type)
        {

            case NetworkListEvent<ChatData>.EventType.Add:
                OnChattingAdd(changeEvent.Value);
                break;

        }

    }

    public void ClearChatting()
    {

        chatList.Clear();

    }


    [ServerRpc(RequireOwnership = false)]
    private void SummitMessageServerRPC(ChatData chatData)
    {

        chatData.userName = HostSingle.Instance.NetServer.GetUserDataByClientID(chatData.clientId).Value.nickName;
        chatData.messageId = chatList.Count + 1;
        chatList.Add(chatData);

    }

    public void SummitMessage(string message)
    {

        if(message.Length > MAX_CHAT_CHAR)
        {

            Debug.LogError("채팅 메시지가 100글자 이상입니다!");
            return;

        }

        ChatData chatData = new ChatData(message, NetworkManager.LocalClientId);

        SummitMessageServerRPC(chatData);


    }

    public NetworkList<ChatData> GetChatting()
    {

        return chatList;

    }

    
}