using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Text;
using Unity.VisualScripting;

public class DebugSC : NetworkBehaviour
{



    void Start()
    {

        if (IsServer)
        {

            var ls = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            var rpLs = new RPCList<int>(ls);

            DebugClientRPC(Support.Serialize<int>(rpLs));

        }



    }

    [ClientRpc]
    public void DebugClientRPC(byte[] stream)
    {

        var ls = stream.Deserialize<int>();

        foreach(var item in ls)
        {

            Debug.Log(item);

        }

    }

}
