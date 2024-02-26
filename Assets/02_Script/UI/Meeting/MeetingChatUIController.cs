using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MeetingChatUIController : MonoBehaviour
{

    [SerializeField] private ChattingSystem chattingSystem;
    [SerializeField] private TMP_InputField chatInput;
    [SerializeField] private MeetingPlayerChatPanel clientPanel;
    [SerializeField] private MeetingPlayerChatPanel ownerPanel;
    [SerializeField] private Transform chatPanelRoot;

    public void Init(bool active)
    {

        if (active)
        {

            var ls = chattingSystem.GetChatting();

            foreach (var c in ls)
            {

                HandleChattingAdd(c);

            }

            chattingSystem.OnChattingAdd += HandleChattingAdd;

        }
        else
        {

            for(int i = 0; i < chatPanelRoot.childCount; i++)
            {

                Destroy(chatPanelRoot.GetChild(i).gameObject);

            }

            chattingSystem.OnChattingAdd -= HandleChattingAdd;

        }

    }

    private void HandleChattingAdd(ChatData chatData)
    {

        var prefab = NetworkManager.Singleton.LocalClientId == chatData.clientId ? ownerPanel : clientPanel;

        Instantiate(prefab, chatPanelRoot).Setting(chatData);

    }

    public void SummitMessage()
    {

        chattingSystem.SummitMessage(chatInput.text);
        chatInput.text = "";

    }

}
