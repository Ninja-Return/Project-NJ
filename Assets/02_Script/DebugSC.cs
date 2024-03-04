using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Text;

public class DebugSC : NetworkBehaviour
{



    void Start()
    {

        if (IsServer)
        {

            var ls = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            var ms = new MemoryStream();
            var bf = new BinaryFormatter();

            bf.Serialize(ms, ls);

            var data = Encoding.UTF8.GetString(ms.GetBuffer());

            Debug.Log(data);

            DebugClientRPC(data);

        }



    }

    [ClientRpc]
    public void DebugClientRPC(FixedString4096Bytes str)
    {

        Debug.Log(str);

        var ms = new MemoryStream(Encoding.UTF8.GetBytes(str.ToString()));
        var bf = new BinaryFormatter();

        List<int> ls = (List<int>)bf.Deserialize(ms);

        foreach(var item in ls)
        {

            Debug.Log(item);

        }

    }

}
