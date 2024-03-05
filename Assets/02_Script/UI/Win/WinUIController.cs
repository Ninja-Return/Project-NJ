using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class WinUIController : NetworkBehaviour
{

    [SerializeField] private TMP_Text winText;

    private void Start()
    {

        if (IsServer)
        {

            EnumWinState state = (EnumWinState)PlayerPrefs.GetInt("WinState");
            Debug.Log(state);
            SetTextClientRPC(GetText(state));

        }

    }

    private string GetText(EnumWinState state)
    {

        switch (state)
        {

            case EnumWinState.None:
                return "ERROR";
            case EnumWinState.Player:
                return "�÷��̾� �¸�";
            case EnumWinState.Mafia:
                return "���Ǿ� �¸�";

        }

        return "???????";

    }

    [ClientRpc]
    private void SetTextClientRPC(FixedString32Bytes str)
    {

        NetworkController.Instance.vivox.LeaveNormalChannel();
        NetworkController.Instance.vivox.Leave3DChannel();
        winText.text = str.ToString();

    }

}
