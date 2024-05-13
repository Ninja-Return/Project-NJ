using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TutorialMoney : TutorialObject
{
    [SerializeField] private NetworkObject ceilObj;
    [SerializeField] private Transform[] spawnTrs;
    [SerializeField] private NetworkObject[] items;

    private CreditSystem creditSystem;

    protected override void Init()
    {
        creditSystem = FindObjectOfType<CreditSystem>();

        ceilObj.gameObject.SetActive(true);
        ceilObj.Spawn();

        foreach (Transform spawn in spawnTrs)
        {
            int idx = Random.Range(0, items.Length);
            NetworkObject item = Instantiate(items[idx], spawn.position, Quaternion.identity);
            item.Spawn();
        }
    }

    protected override void IsClearTutorial()
    {
        if (creditSystem.Credit >= 600)
        {
            isTutorialOn = false;
            TutorialSystem.Instance.StartSequence("Key");
        }
    }
}
