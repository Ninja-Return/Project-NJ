using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TestSpawn : MonoBehaviour
{
    [SerializeField] private NetworkObject networkObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            NetworkObject n = Instantiate(networkObject, transform.position, Quaternion.identity);
            n.Spawn();
        }
    }
}
