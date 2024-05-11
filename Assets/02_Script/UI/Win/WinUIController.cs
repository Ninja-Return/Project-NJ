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

            //EnumWinState state = (EnumWinState)PlayerPrefs.GetInt("WinState");
            //SettingClientRPC(state);

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
                SpawnPanelClientRpc(item, data.Value);

            }

        }

        //PlayerPrefs.SetString("MafiaNickName", "");
    }


    [ClientRpc]
    private void SpawnPanelClientRpc(ulong clientId, UserData data)
    {

        controller.SpawnPanel(clientId, clientId == NetworkManager.LocalClientId, data);

    }

    //[ClientRpc]
    //private void SettingClientRPC(EnumWinState state)
    //{

    //    switch (state)
    //    {

    //        case EnumWinState.None:
    //            controller.EscapeFail();
    //            break;
    //        case EnumWinState.Escape:
    //            controller.EscapeClear();
    //            break;
    //        case EnumWinState.Fail:
    //            controller.EscapeFail();
    //            break;

    //    }


    //}

}
