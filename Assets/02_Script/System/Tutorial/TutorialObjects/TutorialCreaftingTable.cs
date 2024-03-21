using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCreaftingTable : TutorialObject
{
    private CraftingTable table;
    private bool clearCraft;

    private void Start()
    {
        table = FindObjectOfType<CraftingTable>();
        table.OnItemCraftEvt += () => clearCraft = true;
    }

    protected override void IsClearTutorial()
    {
        if (clearCraft)
        {
            isTutorialOn = false;
            TutorialSystem.Instance.StartSequence("Last");
        }
    }
}
