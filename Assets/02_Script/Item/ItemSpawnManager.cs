using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemSpawnManager : NetworkBehaviour
{
    
    public static ItemSpawnManager Instance;

    private void Awake()
    {
        
        Instance = this;

    }

    public void SpawningItem(Vector3 pos, string str)
    {

        SpawningItemServerRPC(pos, str);

    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawningItemServerRPC(Vector3 pos, string itemKey)
    {

        var item = Resources.Load<ItemRoot>($"ItemObj/{itemKey}");

        var clone = Instantiate(item,
            pos,
            Quaternion.identity);

        clone.NetworkObject.Spawn();

    }

}
