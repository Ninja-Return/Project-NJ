using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpawner : NetworkBehaviour
{

    [SerializeField] private NetworkObject monsterPrefab;
    [SerializeField] private List<Transform> spawnPos;

    private NetworkObject monsterObj;

    private void Start()
    {

        if (IsServer)
        {

            DayManager.instance.OnDayComming += HandleDestroyMonster;

            MeetingSystem.Instance.OnMeetingEnd += HandleMonsterSpawn;

        }

    }

    private void HandleDestroyMonster()
    {

        if(monsterObj != null)
        {

            monsterObj.Despawn();

        }

    }

    private void HandleMonsterSpawn()
    {

        var pos = spawnPos.GetRandomListObject().position;

        monsterObj = Instantiate(monsterPrefab, pos, Quaternion.identity);

        monsterObj.Spawn(true);

    }

}
