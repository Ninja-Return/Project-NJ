using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSpawnItem : HandItemRoot
{

    public GameObject monster;

    public override void DoUse()
    {
        PlayerManager.Instance.localController.SpawnMonster(monster, 2f);
    }
}
