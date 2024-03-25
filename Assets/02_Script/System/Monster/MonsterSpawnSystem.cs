using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MonsterSpawnSystem : NetworkBehaviour
{

    [SerializeField] private int monsterMaxSpawn = 5;
    [SerializeField] private NetworkObject monsterPrefab;

    private List<Transform> spawnTrms = new List<Transform>();
    public static MonsterSpawnSystem Instance { get; private set; } 

    private void Awake()
    {
        
        Instance = this;

    }

    private void Start()
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

            yield return new WaitForSeconds(Random.Range(60f, 200f));
            Instantiate(monsterPrefab, spawnTrms.GetRandomListObject().position, Quaternion.identity).Spawn(true);


        }

    }


}
