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
    [Header("SpawnPos")]
    [SerializeField] private List<Transform> spawnTrms;
    [Header("Die")]
    [SerializeField] private DeathUI deathUI;

    private List<PlayerController> players = new();
    public PlayerController localController { get; private set; }

    public static PlayerManager Instance { get; private set; }
    public event Action<ulong> OnPlayerDie;

    public NetworkList<LiveData> alivePlayer { get; private set; }
    public NetworkList<LiveData> diePlayer { get; private set; }

    public bool IsDie { get; private set; }

    private void Awake()
    {

        alivePlayer = new();
        diePlayer = new();

        Instance = this;

    }

    private void Start()
    {

        if (!IsServer || New_GameManager.Instance == null) return;

        New_GameManager.Instance.OnPlayerSpawnCall += HandlePlayerSpawn;
 

    }

    private void HandlePlayerSpawn()
    {

        foreach(var item in NetworkManager.ConnectedClientsIds)
        {

            var spawnTrm = spawnTrms.GetRandomListObject();
            spawnTrms.Remove(spawnTrm);

            var pl = Instantiate(playerPrefab, spawnTrm.position, Quaternion.identity)
                .GetComponent<PlayerController>();

            pl.NetworkObject.SpawnWithOwnership(item, true);

            players.Add(pl);
            var data = HostSingle.Instance.NetServer.GetUserDataByClientID(pl.OwnerClientId).Value;
            alivePlayer.Add(new LiveData { clientId = pl.OwnerClientId, name = data.nickName });

        }

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

    #region ServerRPC

    [ServerRpc(RequireOwnership = false)]
    private void PlayerDieServerRPC(EnumList.DeadType type, ulong clientId) 
    { 
        
        var player = players.Find(x => x.OwnerClientId == clientId);

        if(player == null) return;

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
            PlayerRoleManager.Instance.FindMafiaId() != null
            );


    }




    #endregion

    #region ClientRPC

    [ClientRpc]
    private void PlayerDieClientRPC(EnumList.DeadType type, ClientRpcParams param)
    {

        deathUI.gameObject.SetActive(true);
        deathUI.PopupDeathUI(type);

        IsDie = true;

    }

    #endregion


}
