using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinUIController : MonoBehaviour
{

    [SerializeField] private TMP_Text winText;

    private void Awake()
    {

        winText.text = GetText();

    }

    private string GetText()
    {

        switch (WinSystem.Instance.winState.Value)
        {

            case EnumWinState.None:
                return "ERROR";
            case EnumWinState.Player:
                return "플레이어 승리";
            case EnumWinState.Mafia:
                return "마피아 승리";

        }

        return "???????";

    }

}
