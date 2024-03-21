using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInventory : TutorialObject
{
    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    protected override void IsClearTutorial()
    {
        if (inventory.isShow && inventory.getItemCount > 1)
        {
            isTutorialOn = false;
            TutorialSystem.Instance.StartSequence("Craft");
        }
    }
}
