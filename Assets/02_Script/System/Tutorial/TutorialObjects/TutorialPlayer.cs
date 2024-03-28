using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : TutorialObject
{
    private PlayerController playerController;

    public bool isMove;

    protected override void Init()
    {
        Debug.Log("GET");
        playerController = FindObjectOfType<PlayerController>();
    }

    protected override void IsClearTutorial()
    {
        if (playerController.Input.MoveVecter != Vector2.zero)
        {
            isTutorialOn = false;
            TutorialSystem.Instance.StartSequence("Inventory");
        }
    }
}
