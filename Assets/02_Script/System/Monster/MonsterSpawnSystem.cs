using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[System.Serializable]
public struct MonsterSpawnData
{
    public NetworkObject monster;
    public int spawnRatio;
}

public class MonsterSpawnSystem : NetworkBehaviour
{

    [SerializeField] private int monsterMaxSpawn = 5;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    [SerializeField] private List<MonsterSpawnData> monsterDatas;
    [SerializeField] private NetworkObject statuePrefab;

    private List<NetworkObject> monsterPrefabs = new List<NetworkObject>();
    private List<Transform> spawnTrms = new List<Transform>();

    public static MonsterSpawnSystem Instance { get; private set; } 

    private void Awake()
    {
        
        Instance = this;

    }

    private void Start()
    {
        if (!IsServer) return;

        New_GameManager.Instance.OnHardEvent += HandleStatueSpawn;

        foreach (MonsterSpawnData data in monsterDatas)
        {
            for (int i = 0; i < data.spawnRatio; i++)
            {
                monsterPrefabs.Add(data.monster);
            }
        }

    }

    private void HandleStatueSpawn()
    {

        int r = Random.Range(1, 6);

        for(int i = 0; i < r; i++)
        {

            Instantiate(statuePrefab, spawnTrms.GetRandomListObject().position, Quaternion.identity).Spawn(true);

        }

    }

    public void HandleSpawn()
    {

        if (!IsServer) return;

        StartCoroutine(SpawnCo());

    }

    public void AddTrm(Transform trm)
    {

        spawnTrms.Add(trm);

    }

    private IEnumerator SpawnCo()
    {


        for(int i = 0; i < monsterMaxSpawn; i++)
        {

            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            Instantiate(monsterPrefabs.GetRandomListObject(), spawnTrms.GetRandomListObject().position, Quaternion.identity).Spawn(true);

        }

    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnMonsterServerRPC(Vector3 spawnPos, float time)
    {

        StartCoroutine(MonsterSpawn(spawnPos, time));

    }

    private IEnumerator MonsterSpawn(Vector3 spawnPos, float time)
    {

        yield return new WaitForSeconds(time);
        Instantiate(monsterPrefabs[0], spawnPos + new Vector3(2,0,0), Quaternion.identity).Spawn(true);

    }


}
