using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Support
{

    /// <summary>
    /// 리스트 요소중 랜덤한 하나를 반환합니다
    /// </summary>
    public static T GetRandomListObject<T>(this List<T> list)
    {

        int idx = Random.Range(0, list.Count);

        return list[idx];

    }

    /// <summary>
    /// 내부요소가 랜덤하게 뒤바뀐 리스트를 반환합니다
    /// </summary>
    /// <param name="list">원본 리스트</param>
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

}
