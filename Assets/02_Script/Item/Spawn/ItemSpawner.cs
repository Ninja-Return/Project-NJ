using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    [SerializeField] private List<Transform> spawnPoss;
    [SerializeField] private List<ItemRoot> spawnItemList;
    [SerializeField] private bool debug;

    private void Start()
    {

        if(spawnPoss.Count == 0)
        {

            for(int i = 0;  i < transform.childCount; i++)
            {

                spawnPoss.Add(transform.GetChild(i));

            }

        }

        SpawnItem();

    }

    public void SpawnItem()
    {

        foreach(var pos in spawnPoss)
        {

            if (Random.value < 0.8f)
            {

                var itemPrefab = spawnItemList.GetRandomListObject();

                Instantiate(itemPrefab, pos.position, Quaternion.identity)
                    .NetworkObject.Spawn(true);


            }


        }

    }

}
