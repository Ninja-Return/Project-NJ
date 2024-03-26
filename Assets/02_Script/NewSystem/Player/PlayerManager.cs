using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{

    [Header("Player")]
    [SerializeField] private PlayerController playerPrefab;
    [Header("Die")]
    [SerializeField] private DeathUI deathUI;

    private List<PlayerController> players = new();
    public PlayerController localController { get; private set; }

    public static PlayerManager Instance { get; private set; }
    public event Action<ulong> OnPlayerDie;

    public NetworkList<LiveData> alivePlayer { get; private set; }
    public NetworkList<LiveData> diePlayer { get; private set; }

    public bool IsDie { get; private set; }
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


        if (IsServer)
        {

            HostSingle.Instance.GameManager.OnPlayerConnect += HandlePlayerSpawn;

            StartCoroutine(WaitSpawn());

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

        PlayerDieServerRPC(type,clientId);
        

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

        if(player == null) return;

        if (type == EnumList.DeadType.Escape)
        {
            var data = HostSingle.Instance.NetServer.GetUserDataByClientID(clientId).Value;
            data.isBreak = true;
            HostSingle.Instance.NetServer.SetUserDataByClientId(clientId, data);

            IsBreaken = true;
        }

        players.Remove(player);
        player.NetworkObject.Despawn();

        OnPlayerDie?.Invoke(clientId);

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

        var param = new ClientRpcParams
        {

            Send = new ClientRpcSendParams
            {

                TargetClientIds = new[] { clientId },

            }

        };

        PlayerDieClientRPC(type, param);

        New_GameManager.Instance.CheckGameEnd
            (
            players.Count,
            IsBreaken
            );


    }




    #endregion

    #region ClientRPC

    [ClientRpc]
    private void PlayerDieClientRPC(EnumList.DeadType type, ClientRpcParams param)
    {

        //deathUI.gameObject.SetActive(true);
        //deathUI.PopupDeathUI(type);

        WatchingSystem.Instance.StartWatching();

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

    [ServerRpc(RequireOwnership = false)]
    private void JoinSceneServerRPC()
    {

        joinCount++;

    }

    private IEnumerator WaitSpawn()
    {

        yield return new WaitUntil(() => joinCount == NetworkManager.ConnectedClients.Count);

        foreach (var item in NetworkManager.ConnectedClientsIds)
        {

            SpawnPlayer(item);

        }

    }

}
