using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public delegate void OnSpectatorChatting(ChatData chatData);

public class SpectatorChattingSystem : NetworkBehaviour
{

    private readonly int MAX_CHAT_CHAR = 100;

    private NetworkList<ChatData> SpectatorchatList;
    public event OnSpectatorChatting OnSpectatorChattingAdd;

    private void Awake()
    {
        SpectatorchatList = new();
    }

    private void Start()
    {
        
        if (IsClient)
        {

            SpectatorchatList.OnListChanged += HandleChattingChanged;

        }

    }

    private void HandleChattingChanged(NetworkListEvent<ChatData> changeEvent)
    {

        switch (changeEvent.Type)
        {

            case NetworkListEvent<ChatData>.EventType.Add:
                OnSpectatorChattingAdd?.Invoke(changeEvent.Value);
                break;

        }

    }

    public void ClearSpectatorChatting()
    {

        SpectatorchatList.Clear();

    }

    [ServerRpc(RequireOwnership = false)]
    private void SummitMessageServerRPC(ChatData chatData)
    {

        chatData.userName = HostSingle.Instance.NetServer.GetUserDataByClientID(chatData.clientId).Value.nickName;
        chatData.messageId = SpectatorchatList.Count + 1;
        SpectatorchatList.Add(chatData);

    }

    public void SummitMessage(string message)
    {

        if (message.Length > MAX_CHAT_CHAR)
        {

            Debug.LogError("채팅 메시지가 100글자 이상입니다!");
            return;

        }

        ChatData chatData = new ChatData(message, NetworkManager.LocalClientId);

        SummitMessageServerRPC(chatData);

    }

    public NetworkList<ChatData> GetSpectatorChatting()
    {

        return SpectatorchatList;

    }

}
