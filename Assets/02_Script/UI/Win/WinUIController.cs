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
                return "�÷��̾� �¸�";
            case EnumWinState.Mafia:
                return "���Ǿ� �¸�";

        }

        return "???????";

    }

}
