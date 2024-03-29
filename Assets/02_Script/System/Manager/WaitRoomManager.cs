using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WaitRoomManager : NetworkBehaviour
{

    [SerializeField] private PlayerController prefab;
    [SerializeField] private GameObject uiPanel1;
    [SerializeField] private GameObject uiPanel2;
    [SerializeField] private GameObject loadingPanel;

    public NetworkVariable<bool> IsRunningGame;

    public static WaitRoomManager Instance;

    private void Awake()
    {
        
        Instance = this;
        IsRunningGame = new();

    }

    [ServerRpc]
    public void UnActivePanelServerRpc()
    {
        UnActivePanelClientRpc();
    }

    [ClientRpc]
    public void UnActivePanelClientRpc()
    {

        uiPanel1.SetActive(false);
        uiPanel2.SetActive(false);
        loadingPanel.SetActive(true);

    }

    public void UnActiveLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }

}
