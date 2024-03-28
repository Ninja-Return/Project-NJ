using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialItem : TutorialObject
{
    private PlayerController playerController;
    private PlayerHand playerHand;

    protected override void Init()
    {
        playerHand = FindObjectOfType<PlayerHand>();
        playerController = FindObjectOfType<PlayerController>();
        playerController.Input.OnUseObjectKeyPress += () => { TutorialSystem.Instance.StartSequence("Last"); };
    }

    protected override void IsClearTutorial()
    {
        
    }
}
