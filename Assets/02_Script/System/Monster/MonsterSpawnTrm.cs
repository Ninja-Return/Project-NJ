using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MonsterSpawnTrm : NetworkBehaviour
{

    private void Start()
    {

        if (IsServer)
        {

            MonsterSpawnSystem.Instance.AddTrm(transform);

        }

    }

}
