using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class WinUIController : NetworkBehaviour
{

    [SerializeField] private ResultUIController controller;

    private void Start()
    {

        if (IsServer)
        {

            EnumWinState state = (EnumWinState)PlayerPrefs.GetInt("WinState");
            SettingClientRPC(state);

            PlayerPanelSetting();

        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    private void PlayerPanelSetting()
    {
        foreach (var item in NetworkManager.ConnectedClientsIds)
        {

            var data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(item);

            if (data != null)
            {

                SpawnPanelClientRPC(item, data.Value.nickName, data.Value.isBreak);

            }

        }

        //PlayerPrefs.SetString("MafiaNickName", "");
    }

    [ClientRpc]
    private void SpawnPanelClientRPC(ulong clientId, string userName, bool isBreak)
    {

        controller.SpawnPanel(clientId, userName, clientId == NetworkManager.LocalClientId, isBreak);

    }

    [ClientRpc]
    private void SettingClientRPC(EnumWinState state)
    {

        switch (state)
        {

            case EnumWinState.None:
                controller.EscapeFail();
                break;
            case EnumWinState.Player:
                controller.EscapeClear();
                break;
            case EnumWinState.Mafia:
                controller.EscapeFail();
                break;

        }


    }

}
