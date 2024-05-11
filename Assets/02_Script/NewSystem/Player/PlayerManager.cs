using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class PlayerManager : NetworkBehaviour
{

    [Header("Player")]
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private NetworkObject playerDeadbodyPrefab;
    [Header("Die")]
    [SerializeField] private bool spawn = true;

    public List<PlayerController> players { get; private set; } = new();
    public PlayerController localController { get; private set; }

    public static PlayerManager Instance { get; private set; }
    public event Action<ulong> OnPlayerDie;

    public NetworkList<LiveData> alivePlayer { get; private set; }
    public NetworkList<LiveData> diePlayer { get; private set; }

    public float PlayerTime = 0;
    public bool IsDie { get; private set; }
    public bool active => localController == null ? false : localController.CurrentState != EnumPlayerState.Idle;
    private bool IsBreaken;
    private int joinCount;

    private void Awake()
    {

        alivePlayer = new();
        diePlayer = new();

        Instance = this;

    }

    private void Start()
    {

        if (IsServer && spawn)
        {

            PlayerTime = 0;

            HostSingle.Instance.GameManager.OnPlayerConnect += HandlePlayerSpawn;

            StartCoroutine(WaitSpawn());

        }


    }

    private void Update()
    {
        if (ClearTimeManager.Instance.TimerStarted && !IsDie)
        {
            PlayerTimer time = localController.GetComponent<PlayerTimer>();
            PlayerTime = time.time;

        }
    }

    private void HandlePlayerSpawn(string name, ulong id)
    {

        SpawnPlayer(id);

    }

    private void SpawnPlayer(ulong id)
    {

        var vec = UnityEngine.Random.insideUnitSphere * 2.5f;
        vec.y = 3;

        var pl = Instantiate(playerPrefab, vec, Quaternion.identity)
            .GetComponent<PlayerController>();

        pl.NetworkObject.SpawnWithOwnership(id, true);

        var data = HostSingle.Instance.NetServer.GetUserDataByClientID(id).Value;

        alivePlayer.Add(new LiveData { clientId = id, name = data.nickName });
        players.Add(pl);


    }

    public void PlayerDie(EnumList.DeadType type, ulong clientId)
    {

        PlayerDieServerRPC(type, clientId);

    }

    public void SetLocalPlayer(PlayerController controller)
    {

        localController = controller;

    }

    public void Active(bool value)
    {

        localController.Active(value);

    }

    public PlayerController FindPlayerControllerToID(ulong playerID)
    {
        return players.Find(x => x.OwnerClientId == playerID);
    }

    #region ServerRPC

    [ServerRpc(RequireOwnership = false)]
    private void PlayerDieServerRPC(EnumList.DeadType type, ulong clientId)
    {

        var player = players.Find(x => x.OwnerClientId == clientId);

        if (player == null) return;


        var param = new ClientRpcParams
        {

            Send = new ClientRpcSendParams
            {

                TargetClientIds = new[] { clientId },

            }

        };

        var data = HostSingle.Instance.NetServer.GetUserDataByClientID(clientId).Value;
        data.clearTime = PlayerTime;

        if (type == EnumList.DeadType.Escape)
        {
            data.isBreak = true;
            IsBreaken = true;
        }

        HostSingle.Instance.NetServer.SetUserDataByClientId(clientId, data);

        players.Remove(player);
        player.NetworkObject.Despawn();

        OnPlayerDie?.Invoke(clientId);

        NetworkObject playerDeadbody = Instantiate(playerDeadbodyPrefab, player.transform.position, player.transform.rotation);
        playerDeadbody.Spawn(true);

        var live = new LiveData();

        foreach (var item in alivePlayer)
        {

            if (item.clientId == clientId)
            {

                live = item;
                alivePlayer.Remove(item);
                break;

            }

        }

        diePlayer.Add(live);

        New_GameManager.Instance.CheckGameEnd
            (
            players.Count,
            IsBreaken
            );

        PlayerDieClientRPC(type, players.Count == 0, param);

    }




    #endregion

    #region ClientRPC

    [ClientRpc]
    private void PlayerDieClientRPC(EnumList.DeadType type, bool isLast, ClientRpcParams param)
    {

        NotificationSystem.Instance.Notification("누군가 사망했습니다");

        Inventory.Instance.DropAllItem();

        NetworkController.Instance.vivox.Leave3DChannel();

        if (!isLast)
        {
            DeathUISystem.Instance.PopupDeathUI(type);
            WatchingSystem.Instance.StartWatching();
        }

        IsDie = true;

    }

    #endregion

    public override void OnNetworkSpawn()
    {

        if (IsClient)
        {

            JoinSceneServerRPC();

        }

    }


    public void RequstSpawn(List<Transform> poss)
    {

        foreach (var id in NetworkManager.ConnectedClientsIds)
        {

            var item = poss.GetRandomListObject();
            poss.Remove(item);
            var vec = item.transform.position;

            var pl = Instantiate(playerPrefab, vec, Quaternion.identity)
                .GetComponent<PlayerController>();

            pl.NetworkObject.SpawnWithOwnership(id, true);

            var data = HostSingle.Instance.NetServer.GetUserDataByClientID(id).Value;

            alivePlayer.Add(new LiveData { clientId = id, name = data.nickName });
            players.Add(pl);

        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void JoinSceneServerRPC()
    {

        joinCount++;

    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {

            //alivePlayer.Dispose();
            //diePlayer.Dispose();

            Instance = null;

        }

    }


    private IEnumerator WaitSpawn()
    {

        yield return new WaitUntil(() => joinCount == NetworkManager.ConnectedClients.Count);

        foreach (var item in NetworkManager.ConnectedClientsIds)
        {

            SpawnPlayer(item);

        }

        yield return null;

    }

}
