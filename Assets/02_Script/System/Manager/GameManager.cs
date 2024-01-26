using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

using Random = UnityEngine.Random;

public class GameManager : NetworkBehaviour
{

    [SerializeField] private NetworkObject player;

    public static GameManager Instance;

    public event Action OnGameStarted;

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

    }

}
