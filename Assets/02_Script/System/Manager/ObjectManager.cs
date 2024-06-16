using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class ObjectSet
{

    public string key;
    public NetworkObject obj;

}

public class ObjectManager : NetworkBehaviour
{

    [SerializeField] private List<ObjectSet> objectContainer = new();

    public static ObjectManager Instance { get; private set; }

    public override void OnNetworkSpawn()
    {

        base.OnNetworkSpawn();

        Instance = this;

    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnObjectServerRPC(string key, Vector3 pos, Quaternion rot)
    {

        var obj = objectContainer.Find(x => x.key == key);

        if (obj != null)
        {
            
            Instantiate(obj.obj, pos, rot).Spawn(true);

        }

    }

    public void SpawnObject(string key, Vector3 pos, Quaternion rot)
    {

        SpawnObjectServerRPC(key, pos, rot);

    }

}
