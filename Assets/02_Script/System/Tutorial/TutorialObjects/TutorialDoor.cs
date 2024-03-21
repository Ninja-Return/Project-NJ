using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoor : TutorialObject
{
    private Door door;

    void Start()
    {
        door = FindObjectOfType<Door>();
    }


    protected override void IsClearTutorial()
    {
        if (door.IsDoorOpenning())
        {
            isTutorialOn = false;
            TutorialSystem.Instance.StartSequence("Inventory");
        }
    }
}
