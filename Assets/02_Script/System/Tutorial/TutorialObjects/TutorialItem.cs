using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TutorialItem : TutorialObject
{
    [SerializeField] private NetworkObject soda;
    [SerializeField] private Transform spawnTrs;

    private PlayerController playerController;
    private PlayerHand playerHand;

    protected override void Init()
    {
        playerHand = FindObjectOfType<PlayerHand>();
        playerController = FindObjectOfType<PlayerController>();
        playerController.Input.OnUseObjectKeyPress += ClickLeft;

        NetworkObject item = Instantiate(soda, spawnTrs.position, Quaternion.identity);
        item.Spawn();
    }

    private void ClickLeft()
    {
        Antihypnotic handItem = FindObjectOfType<Antihypnotic>();
        if (handItem == null) return;

        playerController.Input.OnUseObjectKeyPress -= ClickLeft;
        isTutorialOn = false;

        TutorialSystem.Instance.StartSequence("Money");
    }

    protected override void IsClearTutorial()
    {
        
    }
}
