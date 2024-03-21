using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HintSystem : NetworkBehaviour
{

    [SerializeField] private List<Transform> spawnPoss;
    [SerializeField] private HintObject hintPrefab;

    private void Start()
    {

        if (IsServer)
        {

           //New_GameManager.Instance.OnGameStarted += SpawnHint;

        }

    }

    private void SpawnHint()
    {

        var id = PlayerRoleManager.Instance.FindMafiaId();

        var data = HostSingle.Instance.NetServer.GetUserDataByClientID(id.Value).Value;

        foreach(var item in data.attachedItem)
        {

            var obj = Instantiate(hintPrefab, spawnPoss.GetRandomListObject().position, Quaternion.identity);
            obj.NetworkObject.Spawn(true);
            obj.SpawnClientRPC(item.description);

        }

    }
}
