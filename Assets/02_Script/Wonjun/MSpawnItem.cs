using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MSpawnItem : HandItemRoot
{

    [SerializeField] private NetworkObject monsterPrefab;

    public override void DoUse()
    {
        MonsterSpawnSystem.Instance.SpawnMonster(monsterPrefab, 2f);
    }

    

}
