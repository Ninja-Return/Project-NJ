using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpectatorChatPanel : MonoBehaviour
{

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text chatMessage;

    public void SpectatorSetting(ChatData chatData)
    {

        playerName.text = chatData.userName.ToString();
        chatMessage.text = chatData.message.ToString();

    }

}
