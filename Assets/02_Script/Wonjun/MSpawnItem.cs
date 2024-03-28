using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MSpawnItem : HandItemRoot
{
    [SerializeField] private NetworkObject monsterPrefab;
    private void Start()
    {
    }

    public override void DoUse()
    {

        var trmPos = PlayerManager.Instance.localController.transform.position;

        MonsterSpawnSystem.Instance.SpawnMonsterServerRPC(trmPos, 2f);
    }

    

}
