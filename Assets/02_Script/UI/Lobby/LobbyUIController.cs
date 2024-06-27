using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Unity.Services.Lobbies;

public class LobbyUIController : NetworkBehaviour
{

    [SerializeField] private PlayerPanel playerPanelPrefab;
    [SerializeField] private Transform panelRoot;
    [SerializeField] private TMP_Text joinCodeText;
    [SerializeField] private GameObject startBtn;
    [SerializeField] private GameObject mapBtn;

    [SerializeField] private RectTransform joinCodePanel;

    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TMP_Text loadingText;

    private void Start()
    {

        if (IsServer)
        {

            NetworkManager.OnClientConnectedCallback += OnPlayerConnect;
            HostSingle.Instance.GameManager.OnPlayerDisconnect += OnPlayerDisconnect;

            InitPanel();

        }

        startBtn.SetActive(IsHost);
        mapBtn.SetActive(false); //IsHost

        joinCodeText.text = $"�ʴ��ڵ� : {NetworkController.Instance.joinCode}";


        if (HostSingle.Instance.GameManager.gameMode == GameMode.Single)
        {
            joinCodePanel.gameObject.SetActive(false);
        }

    }

    private void OnPlayerDisconnect(string authId, ulong clientId)
    {

        InitPanel();
        HostSingle.Instance.GameManager.UpdateLobby();

    }

    private void OnPlayerConnect(ulong clientId)
    {

        InitPanel();
        HostSingle.Instance.GameManager.UpdateLobby();

    }

    private void InitPanel()
    {

        InitChildClientRPC();

        foreach(var clientId in NetworkManager.ConnectedClientsIds)
        {

            var data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(clientId);

            SetPanelClientRPC(clientId, data.Value.nickName);

        }

    }

    [ClientRpc]
    private void InitChildClientRPC()
    {

        var childs = panelRoot.GetComponentsInChildren<PlayerPanel>();

        foreach (var child in childs)
        {

            Destroy(child.gameObject);

        }

    }

    [ClientRpc]
    private void SetPanelClientRPC(ulong ownerId, string userName)
    {

        var obj = Instantiate(playerPanelPrefab, panelRoot);
        obj.Init(ownerId, userName, IsHost && ownerId != 0);

    }

    [ClientRpc]
    private void PopStartPanelClientRpc()
    {
        PopLoadingPanel("�� ������ ���۵˴ϴ�...");
    }

    public void StartGame()
    {

        PopStartPanelClientRpc();
        HostSingle.Instance.GameManager.ChangeLobbyState(true);
        NetworkManager.SceneManager.LoadScene(SceneList.LoadingScene, UnityEngine.SceneManagement.LoadSceneMode.Single);

    }

    public void BackLobby()
    {
        Debug.Log("click");
        if (IsHost)
        {
            Debug.Log("hostclick");
            PopLoadingPanel("�� ��Ʈ���� ��...");
            HostSingle.Instance.GameManager.ShutdownAsync();
            SceneManager.LoadScene(SceneList.LobbySelectScene);
        }
        else
        {
            Debug.Log("clientclick");
            PopLoadingPanel("�� ������ ��...");
            ClientSingle.Instance.GameManager.Disconnect();
        }
    }

    private void PopLoadingPanel(string extra)
    {
        loadingPanel.SetActive(true);
        loadingText.text = extra;
    }

    private void ShutDownLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }

    public override void OnDestroy()
    {
    
        base.OnDestroy();

        if (IsServer)
        {

            NetworkManager.OnClientConnectedCallback -= OnPlayerConnect;
            HostSingle.Instance.GameManager.OnPlayerDisconnect -= OnPlayerDisconnect;

        }

    }


}
