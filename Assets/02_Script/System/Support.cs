using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Netcode;
using UnityEngine;

public static class Support
{

    /// <summary>
    /// ����Ʈ ����� ������ �ϳ��� ��ȯ�մϴ�
    /// </summary>
    public static T GetRandomListObject<T>(this List<T> list)
    {

        int idx = Random.Range(0, list.Count);

        return list[idx];

    }

    /// <summary>
    /// ���ο�Ұ� �����ϰ� �ڹٲ� ����Ʈ�� ��ȯ�մϴ�
    /// </summary>
    /// <param name="list">���� ����Ʈ</param>
    public static List<T> GetRandomList<T>(this List<T> list, int swapCount, bool copy = true) 
    { 
        
        if(copy)
        {

            list = list.ToList();

        }

        for(int i = 0; i < swapCount; i++)
        {

            int a = Random.Range(0, list.Count);
            int b = Random.Range(0, list.Count);

            (list[a], list[b]) = (list[b], list[a]);

        }



        return list;

    }

    public static RPCList<T> Deserialize<T>(this byte[] bytes) where T : struct 
    {

        var str = Encoding.UTF8.GetString(bytes);

        return JsonUtility.FromJson<RPCList<T>>(str);

    }

    public static byte[] Serialize<T>(RPCList<T> obj) where T : struct
    {

        var str = JsonUtility.ToJson(obj);

        return Encoding.UTF8.GetBytes(str);

    }

}
