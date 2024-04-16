using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyPanel : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text peopleText;

    private string joinCode;

    public void SetPanel(Lobby lobby)
    {

        joinCode = lobby.Data["JoinCode"].Value;
        titleText.text = lobby.Name;
        peopleText.text = $"{lobby.Players.Count}/6";

    }

    public async void JoinButtonClick()
    {

        await AppController.Instance.StartClientAsync(PlayerPrefs.GetString("PlayerName"), joinCode);

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        JoinButtonClick();

    }

}
