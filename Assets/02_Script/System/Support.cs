using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

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

    public static void SettingCursorVisable(bool visable)
    {

        Cursor.lockState = visable ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visable;

    }

    public static RPCList<T> Deserialize<T>(this byte[] bytes) where T : struct 
    {

        var str = Encoding.UTF8.GetString(bytes);


        return JsonUtility.FromJson<RPCList<T>>(str);

    }

    public static byte[] Serialize<T>(this RPCList<T> obj) where T : struct
    {

        var str = JsonUtility.ToJson(obj);

        return Encoding.UTF8.GetBytes(str);

    }

    public static void SetColorAlpha(this TMP_Text text, float value)
    {

        var c = text.color;
        c.a = value;
        text.color = c;

    }

    public static T Find<T>(this NetworkList<T> list, Predicate<T> obj) where T : unmanaged, IEquatable<T>
    {

        foreach(var item in list)
        {

            if (obj(item)) return item;

        }

        return default(T);

    }

    public static void TVEffect(this Transform targetTrm, bool isOn)
    {

        if (isOn)
        {
            targetTrm.localScale = new Vector3(0, 0, 1);

            Sequence tvSequence = DOTween.Sequence();
            tvSequence.Append(targetTrm.DOScaleX(0.1f, 0.2f).SetEase(Ease.InCirc));
            tvSequence.Join(targetTrm.DOScaleY(1f, 0.2f).SetEase(Ease.OutBack));
            tvSequence.Append(targetTrm.DOScaleX(1f, 0.5f).SetEase(Ease.OutElastic));
        }
        else
        {
            targetTrm.DOScale(new Vector3(0, 0, 0), 0.4f).SetEase(Ease.OutExpo);
        }

    }

    public static ClientRpcParams GetRPCParams(this ulong clientId)
    {

        return new ClientRpcParams
        {

            Send = new ClientRpcSendParams
            {

                TargetClientIds = new[] { clientId },
            }

        };

    }


}
