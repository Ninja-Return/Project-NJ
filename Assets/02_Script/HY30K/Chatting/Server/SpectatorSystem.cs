using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class SpectatorSystem : NetworkBehaviour
{

    [SerializeField] private SpectatorUIController chattingUI;
    [SerializeField] private SpectatorChattingSystem chattingSystem;

    private bool isOpening = false;
    
    public static SpectatorSystem Instance { get; private set; }

    public event Action OnChattingEnd;

    private void Awake()
    {

        Instance = this;

    }

    private void Update()
    {
        
        if (GameManager.Instance.isDie)
        {

            HandleChattingOpen();

        }

    }

    private void HandleChattingOpen()
    {

        GameManager.Instance.PlayerMoveableChangeClientRPC(false);

        chattingSystem.ClearSpectatorChatting();

        MettingOpenClientRPC();

    }

    [ClientRpc]
    private void MettingOpenClientRPC()
    {

        if (GameManager.Instance.isDie) return;

        //JoinChannel();

        SoundManager.Play2DSound("MeetingStart");

        DayManager.instance.TimeSetting(true);
        chattingUI.gameObject.SetActive(true);
        chattingUI.ChattingStart();

        GameManager.Instance.clientPlayer.IsMeeting = true;

    }

    public void CloseChatting()
    {

        chattingUI.EndSpectatorChat();

    }

}
