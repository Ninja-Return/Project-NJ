using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnifeItem : HandItemRoot
{

    private PlayerAnimationController animator;
    private bool hold;
    private bool coolDown;

    private void Start()
    {

        animator = PlayerManager.Instance.localController.GetComponent<PlayerAnimationController>();

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

    private IEnumerator CoolDownCo()
    {

        yield return new WaitForSeconds(3);
        coolDown = false;
        hold = false;

    }

}
