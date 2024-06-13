using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMonsterAnimater : MonoBehaviour
{

    private readonly int HASH_JUMPATTACK = Animator.StringToHash("JumpAttack");
    private readonly int HASH_NORMALATTACK = Animator.StringToHash("NormalAttack");
    private readonly int HASH_ISWALK = Animator.StringToHash("IsWalk");
    private readonly int HASH_ISRUN = Animator.StringToHash("IsRun");
    private readonly int HASH_STUN = Animator.StringToHash("Stun");

    public event Action OnAnimeEnd;

    private Animator animator;

    private void Awake()
    {
        
        animator = GetComponent<Animator>();

    }

    public void SetJumpAttack()
    {

        animator.SetTrigger(HASH_JUMPATTACK);

    }

    public void SetNormalAttack()
    {

        animator.SetTrigger(HASH_NORMALATTACK);

    }

    public void SetIsWalk(bool isWalk)
    {

        animator.SetBool(HASH_ISWALK, isWalk);

    }

    public void SetIsRun(bool isRun)
    {

        animator.SetBool(HASH_ISRUN, isRun);

    }

    public void SetStun()
    {

        animator.SetTrigger(HASH_STUN);

    }

    public void AnimeEnd()
    {

        OnAnimeEnd?.Invoke();

    }

    internal void SetRoar()
    {
        throw new NotImplementedException();
    }
}
