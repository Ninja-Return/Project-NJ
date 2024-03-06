using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using DG.Tweening;

public class LobbyUIController : NetworkBehaviour
{

    [SerializeField] private PlayerPanel playerPanelPrefab;
    [SerializeField] private Transform panelRoot;
    [SerializeField] private TMP_Text joinCodeText;
    [SerializeField] private GameObject startBtn;

    [Header("Panel")]
    [SerializeField] private RectTransform mapPanel;

    private void Start()
    {

        if (IsServer)
        {

            NetworkManager.OnClientConnectedCallback += OnPlayerConnect;
            HostSingle.Instance.GameManager.OnPlayerDisconnect += OnPlayerDisconnect;

            InitPanel();

        }

        startBtn.SetActive(IsHost);

        joinCodeText.text = $"초대코드 : {NetworkController.Instance.joinCode}";

    }

    private void OnPlayerDisconnect(string authId, ulong clientId)
    {

        InitPanel();

    }

    private void OnPlayerConnect(ulong clientId)
    {

        InitPanel();

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

    public void StartGame()
    {

        HostSingle.Instance.GameManager.ChangeLobbyState(true);
        NetworkManager.SceneManager.LoadScene(SceneList.GameScene, UnityEngine.SceneManagement.LoadSceneMode.Single);

    }

    public void OnMapPanelMove()
    {
        if (mapPanel.localPosition == Vector3.zero)
        {
            mapPanel.DOLocalMove(new Vector2(0, 1200f), 0.5f).SetEase(Ease.OutExpo);
        }
        else
        {
            mapPanel.DOLocalMove(Vector2.zero, 0.5f).SetEase(Ease.InExpo);
        }
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
