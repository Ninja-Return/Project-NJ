using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public struct StoreSellObject
{

    public ItemDataSO data;
    public int price;
    public int raise;
    public string expText;

}

public class StoreSystem : NetworkBehaviour
{

    [field: SerializeField] public List<StoreSellObject> storeList { get; private set; } = new();

    //NetworkVariable은 class를 넣을 수 없다.
    private Dictionary<string, int> raiseItem = new();

    public bool Buy(ItemDataSO buyItem)
    {

        var credit = Inventory.Instance.GetComponent<CreditSystem>();

        var curItem = storeList.Find(x => x.data == buyItem);

        raiseItem.TryGetValue(curItem.data.itemName, out int value);
        int curPrise = curItem.price + value;

        if (credit.Credit >= curPrise)
        {

            if (Inventory.Instance.ObtainItem(curItem.data, ""))
            {

                credit.Credit -= curPrise;
                RaiseUp(buyItem.itemName);

                return true;
            }

        }

        return false;

    }

    public StoreSellObject? GetStoreData(string itemName)
    {
        StoreSellObject? curItem = storeList.Find(x => x.data.itemName == itemName);
        if (curItem == null) return null;

        return ApplyRaiseStoreSellObject(curItem.Value);
    }

    public List<StoreSellObject> GetStoreData()
    {
        List<StoreSellObject> storeItems = new();
        foreach (var items in storeList)
        {
            storeItems.Add(ApplyRaiseStoreSellObject(items));
        }
        return storeItems;
    }

    private StoreSellObject ApplyRaiseStoreSellObject(StoreSellObject s)
    {
        raiseItem.TryGetValue(s.data.itemName, out int raise);

        return new StoreSellObject
        {
            data = s.data,
            price = s.price + raise,
            raise = s.raise,
            expText = s.expText
        };
    }

    public void RaiseUp(string itemName)
    {
        StoreSellObject? curItem = storeList.Find(x => x.data.itemName == itemName);
        if (curItem == null) return;

        if (raiseItem.TryGetValue(itemName, out int raise))
        {
            SetRaiseItemServerRpc(itemName, curItem.Value.raise);
        }
        else
        {
            SetRaiseItemServerRpc(itemName, curItem.Value.raise, true);
        }
    }

    public void RaiseDown(string itemName)
    {
        if (raiseItem.TryGetValue(itemName, out int raise))
        {
            var curItem = storeList.Find(x => x.data.itemName == itemName);
            SetRaiseItemServerRpc(itemName, curItem.raise);
        }
    }

    [ServerRpc]
    private void SetRaiseItemServerRpc(string itemName, int value, bool isEmpty = false)
    {
        SetRaiseItemClientRpc(itemName, value, isEmpty);
    }

    [ClientRpc]
    private void SetRaiseItemClientRpc(string itemName, int value, bool isEmpty = false)
    {
        if (isEmpty)
        {
            raiseItem[itemName] = value;
        }
        else
        {
            raiseItem[itemName] += value;
        }
    }

}
