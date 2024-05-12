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
    //private Dictionary<string, int> raiseItem;

    public bool Buy(ItemDataSO buyItem)
    {

        var credit = Inventory.Instance.GetComponent<CreditSystem>();

        StoreSellObject? curItem = storeList.Find(x => x.data == buyItem);

        if (curItem == null) return false;

        RaiseSystem.Instance.raiseItem.TryGetValue(buyItem.itemName, out int value);
        int curPrise = curItem.Value.price + value;

        if (credit.Credit >= curPrise)
        {

            if (Inventory.Instance.ObtainItem(buyItem, ""))
            {

                credit.Credit -= curPrise;

                RaiseSystem.Instance.RaiseUp(buyItem.itemName, curItem.Value.raise);

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
        RaiseSystem.Instance.raiseItem.TryGetValue(s.data.itemName, out int raise);

        return new StoreSellObject
        {
            data = s.data,
            price = s.price + raise,
            raise = s.raise,
            expText = s.expText
        };
    }
}
