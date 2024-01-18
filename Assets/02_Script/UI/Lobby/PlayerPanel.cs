using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{

    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private GameObject kickButton;

    private ulong ownerId;

    public void Init(ulong ownerId, string userName, bool buttonAble)
    {

        this.ownerId = ownerId;
        playerNameText.text = userName;

        kickButton.SetActive(buttonAble);

        playerNameText.color = ownerId == NetworkManager.Singleton.LocalClientId ? Color.yellow : Color.white;

    }

    public void Kick()
    {

        NetworkManager.Singleton.DisconnectClient(ownerId);

    }

}
