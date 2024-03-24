using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{

    [SerializeField] private List<Transform> spawnPoss;
    [SerializeField] private List<ItemRoot> spawnItems;
    [SerializeField] private bool debug;
    [SerializeField] private int spawnCount;
    [SerializeField, Range(0f, 1f)] private float spawnPercentage = 1;

    private void Start()
    {

        if (IsServer)
        {

            New_GameManager.Instance.OnGameStarted += Spawning;

        }


    }

    private void Spawning()
    {


        if (spawnPoss.Count == 0)
        {

            for (int i = 0; i < transform.childCount; i++)
            {

                spawnPoss.Add(transform.GetChild(i));

            }

        }

        SpawnItem();


    }

    public void SpawnItem()
    {


        for(int i = 0; i < spawnCount; i++)
        {

            if(Random.value <= spawnPercentage)
            {

                var pos = spawnPoss.GetRandomListObject();
                spawnPoss.Remove(pos);

                var itemPrefab = spawnItems.GetRandomListObject();
                Instantiate(itemPrefab, pos.position, pos.transform.rotation)
                    .NetworkObject.Spawn(true);

            }


        }


    }

}
