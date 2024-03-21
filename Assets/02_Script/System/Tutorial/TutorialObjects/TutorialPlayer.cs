using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : TutorialObject
{
     private PlayerController playerController;

    public bool isMove;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        
    }

    protected override void IsClearTutorial()
    {
        if (playerController.Input.MoveVecter != Vector2.zero)
        {
            isTutorialOn = false;
            TutorialSystem.Instance.StartSequence("Door");
        }
    }
}
