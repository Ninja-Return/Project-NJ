using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WaitRoomManager : NetworkBehaviour
{

    [SerializeField] private PlayerController prefab;

    private void Start()
    {

        if (IsServer)
        {

            HostSingle.Instance.GameManager.OnPlayerConnect += HandlePlayerSpawn;

            foreach(var item in NetworkManager.ConnectedClientsIds)
            {

                SpawnPlayer(item);

            }

        }

    }

    private void HandlePlayerSpawn(string name, ulong id)
    {

        SpawnPlayer(id);

    }

    private void SpawnPlayer(ulong id)
    {

        var vec = UnityEngine.Random.insideUnitSphere * 4;
        vec.y = 3;

        var pl = Instantiate(prefab, vec, Quaternion.identity)
    .GetComponent<PlayerController>();

        pl.NetworkObject.SpawnWithOwnership(id, true);


    }

}
