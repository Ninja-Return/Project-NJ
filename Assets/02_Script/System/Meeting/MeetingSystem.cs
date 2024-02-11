using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MeetingSystem : NetworkBehaviour
{

    [SerializeField] private GameObject mettingUI;
    [SerializeField] private Transform panelRoot;
    [SerializeField] private MeetingPanel panelPrefab;

    private void Start()
    {

        if (!IsServer) return;

        DayManager.instance.OnDayComming += HandleMettingOpen;
        DayManager.instance.OnNightComming += HandleMettingOpen;

    }

    private void HandleMettingOpen()
    {

        MettingOpenClientRPC();

        foreach(var item in NetworkManager.ConnectedClientsIds)
        {

            var data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(item);

            if(data != null)
            {

                SpawnPanelClientRPC(item, data.Value.nickName);

            }

        }

    }

    [ClientRpc]
    private void MettingOpenClientRPC()
    {

        DayManager.instance.TimeSetting(true);
        mettingUI.SetActive(true);

    }

    [ClientRpc]
    private void SpawnPanelClientRPC(ulong clientId, string userName)
    {

        Instantiate(panelPrefab, panelRoot).Setting(clientId, userName, clientId == NetworkManager.LocalClientId);

    }

}
