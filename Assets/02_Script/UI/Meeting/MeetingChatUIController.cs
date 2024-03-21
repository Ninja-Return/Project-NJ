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

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {

            SummitMessage();

        }

    }

    public void Init()
    {


        for (int i = 0; i < chatPanelRoot.childCount; i++)
        {

            Destroy(chatPanelRoot.GetChild(i).gameObject);

        }

        var ls = chattingSystem.GetChatting();

        foreach (var c in ls)
        {

            HandleChattingAdd(c);

        }

        chattingSystem.OnChattingAdd += HandleChattingAdd;

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

    public void EndVote()
    {

        chattingSystem.OnChattingAdd -= HandleChattingAdd;

    }

}
