using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrapItem : HandItemRoot
{
    [SerializeField] private TrapDrop trapObj;

    public override void DoUse()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkSoundManager.Play3DSound("ItemThrow", transform.position, 0.1f, 10f);
            var obj = Instantiate(trapObj, transform.root.position + new Vector3(0, 0.5f, 0) + transform.root.forward, Quaternion.identity);
            obj.NetworkObject.Spawn(true);
            obj.SetUp(transform.root.forward);
        }
    }
}
