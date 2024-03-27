using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LobbyUIController : NetworkBehaviour
{

    [SerializeField] private PlayerPanel playerPanelPrefab;
    [SerializeField] private Transform panelRoot;
    [SerializeField] private TMP_Text joinCodeText;
    [SerializeField] private GameObject startBtn;

    [Header("Panel")]
    [SerializeField] private RectTransform peoplePanel;
    [SerializeField] private RectTransform gameSettingPanel;
    [SerializeField] private RectTransform joinCodePanel;
    [SerializeField] private RectTransform mapPanel;
    [SerializeField] private RectTransform gameBarPanel;
    [SerializeField] private Image[] gameBarBtns;

    private bool IsSingleMode;

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

        SetupPanel();

        if (HostSingle.Instance.GameManager.gameMode == GameMode.Single)
        {
            joinCodePanel.gameObject.SetActive(false);
        }

    }

    private void SetupPanel()
    {
        Sequence startSequence = DOTween.Sequence();
        startSequence.Append(peoplePanel.DOLocalMove(Vector2.zero, 0.5f));
        startSequence.Join(gameSettingPanel.DOLocalMove(Vector2.zero, 0.5f));
        startSequence.Join(joinCodePanel.DOLocalMove(Vector2.zero, 0.5f));
        startSequence.Join(gameBarPanel.DOLocalMove(Vector2.zero, 0.5f));
        for (float i = 0; i < gameBarBtns.Length; i++)
        {
            startSequence.Insert(2f + (i * 0.25f), gameBarBtns[(int)i].DOFade(1, 0.5f));
        }
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
        NetworkManager.SceneManager.LoadScene(SceneList.LoadingScene, UnityEngine.SceneManagement.LoadSceneMode.Single);

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

    public void BackLobby()
    {
        Debug.Log("click");
        if (IsHost)
        {
            Debug.Log("hostclick");
            HostSingle.Instance.GameManager.ShutdownAsync();
            SceneManager.LoadScene(SceneList.LobbySelectScene);
        }
        else
        {
            Debug.Log("clientclick");
            ClientSingle.Instance.GameManager.Disconnect();
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
