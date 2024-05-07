using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieTransition : MonsterTransitionRoot
{
    public DieTransition(MonsterFSM controller, MonsterState nextState) : base(controller, nextState) { }

    protected override bool CheckTransition()
    {
        if (monsterFSM.IsDead)
        {
            //monsterFSM.monsterAnim.SetAnimation("Death");
            return true;
        }
        return false;
    }
}
