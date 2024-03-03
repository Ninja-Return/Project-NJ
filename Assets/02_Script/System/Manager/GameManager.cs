using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : NetworkBehaviour
{

    [SerializeField] private NetworkObject player;

    public NetworkList<ulong> alivePlayer { get; private set; } = new();
    public NetworkList<ulong> diePlayer { get; private set; } = new();

    private List<PlayerController> players = new();

    public static GameManager Instance;

    public event Action OnGameStarted;
    public bool PlayerMoveable { get; private set; } = true;


    private void Awake()
    {
        
        Instance = this;

    }

    private IEnumerator Start()
    {

        yield return new WaitForSeconds(1);

        OnGameStarted?.Invoke();

        if (IsServer)
        {

            StartGame();
            HostSingle.Instance.GameManager.OnPlayerConnect += HandlePlayerConnect;

        }

    }

    private void HandlePlayerConnect(string authId, ulong clientId)
    {

        SpawnPlayer(clientId);

    }

    private void StartGame()
    {

        foreach(var id in NetworkManager.ConnectedClientsIds)
        {

            SpawnPlayer(id);

        }

    }

    public override void OnDestroy()
    {
        
        base.OnDestroy();

        Instance = null;

    }

    public void SpawnPlayer(ulong clientId)
    {

        var pl = Instantiate(player);
        pl.transform.position = new Vector3(Random.Range(-10f, 10f), 1f, Random.Range(-10f, 10f));
        pl.SpawnWithOwnership(clientId, true);
        players.Add(pl.GetComponent<PlayerController>());
        alivePlayer.Add(pl.OwnerClientId);

    }

    [ClientRpc]
    public void PlayerMoveableChangeClientRPC(bool value)
    {

        PlayerMoveable = value;

    }

    public void PlayerDie(ulong clientId)
    {

        players.Find(x => x.OwnerClientId == clientId).NetworkObject.Despawn();

        var data = HostSingle.Instance.NetServer.GetUserDataByClientID(clientId).Value;
        data.isDie = true;

        HostSingle.Instance.NetServer.SetUserDataByClientId(clientId, data);

        var param = new ClientRpcParams
        {

            Send = new ClientRpcSendParams
            {

                TargetClientIds = new[] { clientId },

            }

        };

        alivePlayer.Remove(clientId);
        diePlayer.Add(clientId);

        PlayerDieClientRPC(param);

    }

    [ClientRpc]
    private void PlayerDieClientRPC(ClientRpcParams param)
    {

        WatchingSystem.Instance.StartWatching();

    }

}
