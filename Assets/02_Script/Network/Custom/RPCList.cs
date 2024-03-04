using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public struct RPCList<T> : IEnumerable where T : struct
{

    public List<T> list;

    public RPCList(int size = 100)
    {

        list = new(size);

    }

    public RPCList(List<T> list)
    {

        this.list = list;

    }

    public IEnumerator GetEnumerator()
    {

        return list.GetEnumerator();

    }
}
