using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{

    [SerializeField] private NetworkObject player;

    public static GameManager Instance;

    private void Awake()
    {
        
        Instance = this;

    }

    private void Start()
    {

        if (IsServer)
        {

            StartGame();
            HostSingle.Instance.GameManager.OnPlayerConnect += HandlePlayerConnect;

        }

    }

    private void HandlePlayerConnect(string authId, ulong clientId)
    {

        var pl = Instantiate(player);

        pl.transform.position = new Vector3(Random.Range(-10f, 10f), 1f, Random.Range(-10f, 10f));

        pl.SpawnWithOwnership(clientId, true);

    }

    private void StartGame()
    {

        foreach(var id in NetworkManager.ConnectedClientsIds)
        {

            var pl = Instantiate(player);

            pl.transform.position = new Vector3(Random.Range(-10f, 10f), 1f, Random.Range(-10f, 10f));

            pl.SpawnWithOwnership(id, true);

        }

    }

    public override void OnDestroy()
    {
        
        base.OnDestroy();

        Instance = null;

    }

}
