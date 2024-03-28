using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInventory : TutorialObject
{
    [SerializeField] private float getItemCount;
    private Inventory inventory;

    protected override void Init()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    protected override void IsClearTutorial()
    {
        if (inventory.isShow && inventory.getItemCount >= getItemCount)
        {
            isTutorialOn = false;
            TutorialSystem.Instance.StartSequence("Item");
        }
    }
}
