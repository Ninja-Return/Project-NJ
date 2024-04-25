using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareItemSpawner : ItemSpawner
{

    public override void OnItemSpawn(ItemRoot item)
    {

        item.DestroyCallback += HandleRareItemDestroyd;

    }

    private void HandleRareItemDestroyd()
    {

        New_GameManager.Instance.GameToHard();

    }

}
