using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialItem : TutorialObject
{
    [SerializeField] private GameObject soda;
    private PlayerController playerController;
    private PlayerHand playerHand;

    protected override void Init()
    {
        playerHand = FindObjectOfType<PlayerHand>();
        playerController = FindObjectOfType<PlayerController>();
        playerController.Input.OnUseObjectKeyPress += ClickLeft;

        Vector3 pos = soda.transform.position;
        pos.y = 3.0f;
        soda.transform.position = pos;
    }

    private void ClickLeft()
    {
        Soda handItem = FindObjectOfType<Soda>();
        if (handItem == null) return;

        playerController.Input.OnUseObjectKeyPress -= ClickLeft;
        isTutorialOn = false;
        TutorialSystem.Instance.StartSequence("Money");
    }

    protected override void IsClearTutorial()
    {
        
    }
}
