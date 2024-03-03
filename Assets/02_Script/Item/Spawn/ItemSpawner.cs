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

        if (debug)
        {

            SpawnItem();

        }

    }

    public void SpawnItem()
    {

        foreach(var pos in spawnPoss)
        {


            if (Random.value < 0.8f)
            {

                var itemPrefab = spawnItemList.GetRandomListObject();

                if (debug)
                {

                    Instantiate(itemPrefab, pos.position, Quaternion.identity);

                }
                else
                {

                    Instantiate(itemPrefab, pos.position, Quaternion.identity).NetworkObject.Spawn(true);

                }


            }


        }

    }

}
