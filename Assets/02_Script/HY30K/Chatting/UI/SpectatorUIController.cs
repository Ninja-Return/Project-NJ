using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Profiling;

public class SpectatorUIController : MonoBehaviour
{

    [SerializeField] SpectatorChatUIController spectatorChattingPanel;

    public void ChattingStart()
    {

        New_GameManager.Instance.SettingCursorVisable(true);
        spectatorChattingPanel.Init();

    }

    public void EndSpectatorChat()
    {
        
        spectatorChattingPanel.EndSpectatorChat();

    }

}
