using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SpectatorChatUIController : MonoBehaviour
{

    [SerializeField] private ChattingSystem spectatorChattingSystem;
    [SerializeField] private TMP_InputField chatInput;
    [SerializeField] private SpectatorChatPanel clientPanel;
    [SerializeField] private SpectatorChatPanel ownerPanel;
    [SerializeField] private Transform chatPanelRoot;

    public void Init()
    {

        for (int i = 0; i < chatPanelRoot.childCount; i++)
        {

            Destroy(chatPanelRoot.GetChild(i).gameObject);

        }

        var ls = spectatorChattingSystem.GetChatting();

        foreach (var c in ls)
        {

            HandleSpectatorChattingAdd(c);

        }

        spectatorChattingSystem.OnChattingAdd += HandleSpectatorChattingAdd;

    }

    private void HandleSpectatorChattingAdd(ChatData chatData)
    {

        var prefab = NetworkManager.Singleton.LocalClientId == chatData.clientId ? ownerPanel : clientPanel;

        Instantiate(prefab, chatPanelRoot).SpectatorSetting(chatData);

    }

    public void SummitMessage()
    {

        spectatorChattingSystem.SummitMessage(chatInput.text);
        chatInput.text = "";

    }

    public void EndSpectatorChat()
    {

        spectatorChattingSystem.OnChattingAdd -= HandleSpectatorChattingAdd;

    }

}
