using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnifeItem : HandItemRoot
{

    private PlayerAnimationController animator;
    private PlayerEventSystem eventSystem;
    private bool hold;
    private bool coolDown;

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
        NetworkSoundManager.Play3DSound("KnifeSound", transform.position, 0.01f, 15f);

        var players = Physics.OverlapBox(
            transform.position, Vector3.one / 4, 
            Quaternion.identity, LayerMask.GetMask("Player"));

        foreach(var item in players)
        {

            if(item.TryGetComponent<PlayerController>(out var compo))
            {

                if (compo.IsOwner) continue;

                var credit = PlayerManager.Instance.localController.GetComponent<CreditSystem>();
                credit.Credit += 150;

                PlayerManager.Instance.PlayerDie(EnumList.DeadType.Knife, compo.OwnerClientId);
                Inventory.Instance.Deleteltem();

                break;

            }

        }

    }

    public override void DoUse()
    {

        if (coolDown || hold) return;

        animator.InitHandTarget();  
        hold = true;
        animator.PlayTweenAnimation("Knife_Hold");

    }

    public override void DoRelease()
    {

        if (hold == false || coolDown) return;

        coolDown = true;

        animator.PlayTweenAnimation("Knife_Attack");

        StartCoroutine(CoolDownCo());

    }

    private void OnDestroy()
    {

        if (!isOwner) return;
        if(eventSystem != null)
        {

            eventSystem.OnTweenAnimeEvent -= HandleAnimeEnd;

        }

    }

    private IEnumerator CoolDownCo()
    {

        yield return new WaitForSeconds(3);
        coolDown = false;
        hold = false;

    }

}
