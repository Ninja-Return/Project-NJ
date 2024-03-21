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

        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    [ClientRpc]
    private void SettingClientRPC(EnumWinState state)
    {

        switch (state)
        {

            case EnumWinState.None:
                controller.MafiaWin();
                break;
            case EnumWinState.Player:
                controller.HumanWin();
                break;
            case EnumWinState.Mafia:
                controller.MafiaWin();
                break;

        }


    }

}
