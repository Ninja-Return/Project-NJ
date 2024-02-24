using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    public List<string> items;

}


public class AttachedItemSystem : NetworkBehaviour
{

    [SerializeField] private List<AttachedItemData> datas;
    [SerializeField] private TMP_Text debugText;

    private void Start()
    {

        GameManager.Instance.OnGameStarted += HandleGameStarted;

    }

    private void HandleGameStarted()
    {

        if (!IsServer) return;

        var ls = datas.GetRandomList();

        var com = GetCombinations(ls.items, 2);
        var filterList = FilterCombinations(com);

        var randList = Support.GetRandomList(filterList, 1000);

        foreach(var client in NetworkManager.ConnectedClientsIds)
        {

            var data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(client).Value;
            data.attachedItem = randList[(int)client].ToList();

            HostSingle.Instance.GameManager.NetServer.SetUserDataByClientId(client, data);
            SetAttachedItemClientRPC(string.Join(", ", randList[(int)client]), client);

        }

    }

    [ClientRpc]
    private void SetAttachedItemClientRPC(string item, ulong clientId)
    {

        if (NetworkManager.LocalClientId != clientId) return;

        debugText.text = item;

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

    private List<List<string>> FilterCombinations(List<List<string>> combinations)
    {

        var filteredCombinations = new List<List<string>>();

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
