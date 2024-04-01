using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMoney : TutorialObject
{
    [SerializeField] private Rigidbody itemWallRig;
    [SerializeField] private Collider itemWallCol;
    private CreditSystem creditSystem;

    protected override void Init()
    {
        creditSystem = FindObjectOfType<CreditSystem>();
        itemWallRig.useGravity = true;
        itemWallCol.enabled = false;
    }

    protected override void IsClearTutorial()
    {
        if (creditSystem.Credit >= 500)
        {
            isTutorialOn = false;
            TutorialSystem.Instance.StartSequence("Key");
        }
    }
}
