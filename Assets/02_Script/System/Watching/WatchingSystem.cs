using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class WatchingSystem : NetworkBehaviour
{

    [SerializeField] private WatchingUIController watchingUI;
    [SerializeField] private SpectatorUIController chattingUI;
    [SerializeField] private SpectatorChattingSystem chattingSystem;

    public static WatchingSystem Instance;

    private ulong currentWatching;
    private bool isWatching;
    private List<PlayerController> alivePlayers = new();

    private void Awake()
    {

        Instance = this;

    }

    private void Start()
    {

        GameManager.Instance.alivePlayer.OnListChanged += HandleListChanged;

    }

    private void HandleListChanged(NetworkListEvent<LiveData> changeEvent)
    {

        if (!isWatching) return;

        switch (changeEvent.Type)
        {

            case NetworkListEvent<LiveData>.EventType.Remove:
                {

                    alivePlayers.Remove(alivePlayers.Find(x => x.OwnerClientId == changeEvent.Value.clientId));

                    break;

                }

        }

        if (changeEvent.Value.clientId == currentWatching && alivePlayers.Count != 0)
        {

            Watching(alivePlayers[0].OwnerClientId);

        }

    }

    public void Watching(ulong watch)
    {

        if (alivePlayers.Find(x => x.OwnerClientId == currentWatching) != null)
        {

            alivePlayers.Find(x => x.OwnerClientId == currentWatching).watchCam.Priority = -1;

        }

        alivePlayers.Find(x => x.OwnerClientId == watch).watchCam.Priority = 1000;
        currentWatching = watch;

    }

    public void StartWatching()
    {

        HandleChattingOpen();

        // 일반 음성 채널 조인
        JoinVivox();

        isWatching = true;

        alivePlayers = FindObjectsOfType<PlayerController>().ToList();

        Watching(alivePlayers[0].OwnerClientId);

        watchingUI.gameObject.SetActive(true);
        watchingUI.Init();

        GameManager.Instance.SettingCursorVisable(true);

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

    private async void JoinVivox()
    {

        await NetworkController.Instance.vivox.JoinNormalChannel();

    }

}
