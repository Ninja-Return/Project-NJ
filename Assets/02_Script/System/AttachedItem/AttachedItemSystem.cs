using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public enum AttachedItemCategory
{

    Fruit

}

[Serializable]
public class AttachedItemData
{

    public AttachedItemCategory category;
    public List<AttachedItem> items;

}

[Serializable]
public struct AttachedItem
{

    public string name;
    public string description;

    public override string ToString()
    {

        return name;

    }

}

public struct AttachedItemRPCData : INetworkSerializable
{

    public FixedString32Bytes item_1;
    public FixedString32Bytes item_2;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {

        serializer.SerializeValue(ref item_1);
        serializer.SerializeValue(ref item_2);

    }

}

public class AttachedItemSystem : NetworkBehaviour
{

    [SerializeField] private List<AttachedItemData> datas;
    [SerializeField] private AttachedItemUIController controller;

    private void Start()
    {

        GameManager.Instance.OnGameStarted += HandleGameStarted;

    }

    private void HandleGameStarted()
    {

        if (!IsServer) return;

        var ls = datas.GetRandomListObject();

        var com = GetCombinations(ls.items, 2);
        var filterList = FilterCombinations(com);

        var randList = Support.GetRandomList(filterList, 1000);

        foreach(var client in NetworkManager.ConnectedClientsIds)
        {

            var data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(client).Value;
            data.attachedItem = randList[(int)client].ToList();

            HostSingle.Instance.GameManager.NetServer.SetUserDataByClientId(client, data);
            SetAttachedItemClientRPC(new AttachedItemRPCData
            {

                item_1 = data.attachedItem[0].ToString(),
                item_2 = data.attachedItem[1].ToString()

            }, client.GetRPCParams());

        }

    }

    [ClientRpc]
    private void SetAttachedItemClientRPC(AttachedItemRPCData data, ClientRpcParams clientRpcParams)
    {

        controller.Init(data);

    }

    private List<List<T>> GetCombinations<T>(List<T> list, int length)
    {

        if (length == 1) return list.Select(item => new List<T> { item }).ToList();

        var combinations = new List<List<T>>();

        for (int i = 0; i < list.Count; i++)
        {
            var remaining = GetCombinations(list.Skip(i + 1).ToList(), length - 1);
            foreach (var next in remaining)
            {

                var combination = new List<T> { list[i] };

                combination.AddRange(next);
                combinations.Add(combination);

            }
        }

        return combinations;

    }

    private List<List<AttachedItem>> FilterCombinations(List<List<AttachedItem>> combinations)
    {

        var filteredCombinations = new List<List<AttachedItem>>();

        foreach (var combination in combinations)
        {

            if (filteredCombinations.All(fc => fc.Intersect(combination).Count() <= 1))
            {

                filteredCombinations.Add(combination);

            }

        }

        return filteredCombinations;
    }

}
