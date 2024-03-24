using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public struct StoreSellObject
{

    public ItemDataSO data;
    public int price;
    public string expText;

}

public class StoreSystem : NetworkBehaviour
{

    [field: SerializeField] public List<StoreSellObject> storeList { get; private set; } = new();

    public bool Buy(ItemDataSO buyItem)
    {

        var credit = Inventory.Instance.GetComponent<CreditSystem>();

        var curItem = storeList.Find(x => x.data == buyItem);

        if (credit.Credit >= curItem.price)
        {

            if(Inventory.Instance.ObtainItem(curItem.data, ""))
            {

                credit.Credit -= curItem.price;

            }

        }

        return false;

    }

}
