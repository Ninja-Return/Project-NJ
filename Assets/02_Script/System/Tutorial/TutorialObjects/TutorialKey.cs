using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoxrialKey : TutorialObject
{
    private CreditSystem creditSystem;

    protected override void Init()
    {
        creditSystem = FindObjectOfType<CreditSystem>();
    }

    protected override void IsClearTutorial()
    {
        if (creditSystem.Credit < 500)
        {
            isTutorialOn = false;
            TutorialSystem.Instance.StartSequence("Last");
        }
    }
}
