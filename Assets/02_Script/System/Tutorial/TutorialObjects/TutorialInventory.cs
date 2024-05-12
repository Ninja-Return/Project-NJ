using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TutorialInventory : TutorialObject
{
    [SerializeField] private NetworkObject Item;
    [SerializeField] private Transform spawnTrs;
    [SerializeField] private string itemName;

    private Inventory inventory;
    private bool isCheck;

    protected override void Init()
    {
        inventory = FindObjectOfType<Inventory>();

        NetworkObject item = Instantiate(Item, spawnTrs.position, Quaternion.identity);
        item.Spawn();
    }

    protected override void IsClearTutorial()
    {
        if (inventory.isShow && inventory.GetItem(itemName))
        {
            isCheck = true;
        }

        if (isCheck && !inventory.isShow)
        {
            isTutorialOn = false;
            isCheck = false;
            TutorialSystem.Instance.StartSequence("Item");
        }
    }
}
