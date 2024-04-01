using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInventory : TutorialObject
{
    [SerializeField] private GameObject soda;
    [SerializeField] private float getItemCount;
    private Inventory inventory;
    private bool isCheck;

    protected override void Init()
    {
        inventory = FindObjectOfType<Inventory>();

        Vector3 pos = soda.transform.position;
        pos.y = 3.0f;
        soda.transform.position = pos;
    }

    protected override void IsClearTutorial()
    {
        if (inventory.isShow && inventory.getItemCount >= getItemCount)
        {
            isCheck = true;
        }

        if (isCheck && !inventory.isShow)
        {
            isTutorialOn = false;
            TutorialSystem.Instance.StartSequence("Item");
        }
    }
}
