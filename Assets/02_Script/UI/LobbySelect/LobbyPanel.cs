using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyPanel : MonoBehaviour, IPointerDownHandler
{
    private LobbySelectUIController uiController;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text peopleText;

    private string joinCode;

    public void SetPanel(Lobby lobby, LobbySelectUIController controller)
    {

        uiController = controller;
        joinCode = lobby.Data["JoinCode"].Value;
        titleText.text = lobby.Name;
        peopleText.text = $"{lobby.Data["Players"].Value}/6";
        
    }

    public async void JoinButtonClick()
    {

        uiController.PopLoadingPanel("코드로 참가 중...");

        bool isConnected = await AppController.Instance.StartClientAsync(PlayerPrefs.GetString("PlayerName"), joinCode);
        if (!isConnected)
        {
            uiController.ShutDownLoadingPanel();
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {


    }

}
