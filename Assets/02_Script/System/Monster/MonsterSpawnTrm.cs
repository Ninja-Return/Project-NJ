using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MonsterSpawnTrm : NetworkBehaviour
{

    public override void OnNetworkSpawn()
    {

        if (IsServer)
        {

            MonsterSpawnSystem.Instance.AddTrm(transform);

        }

    }

}
