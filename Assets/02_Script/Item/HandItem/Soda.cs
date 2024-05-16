using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soda : HandItemRoot
{

    private PlayerAnimationController animator;
    private PlayerEventSystem eventSystem;
    private bool played;

    private void Start()
    {

        if (!isOwner) return;

        var player = PlayerManager.Instance.localController;

        animator = player.GetComponent<PlayerAnimationController>();
        eventSystem = player.GetComponent<PlayerEventSystem>();
        eventSystem.OnTweenAnimeEvent += HandleAnimeEnd;

    }

    private void HandleAnimeEnd()
    {

        PlayerManager.Instance.localController.AddSpeed(3, 8);
        Inventory.Instance.Deleteltem();

    }

    public override void DoUse()
    {

        if(played) return;
        played = true;

        animator.PlayTweenAnimation("Soda");

        NetworkSoundManager.Play3DSound("SodaOpen", transform.position, 0.01f, 15f);

    }

    private void OnDestroy()
    {

        if (!isOwner) return;
        if (eventSystem != null)
        {

            eventSystem.OnTweenAnimeEvent -= HandleAnimeEnd;

        }

    }

}
