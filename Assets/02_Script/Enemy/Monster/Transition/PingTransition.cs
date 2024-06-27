using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingTransition : MonsterTransitionRoot
{
    public PingTransition(MonsterFSM controller, MonsterState nextState) : base(controller, nextState) { }

    protected override bool CheckTransition()
    {
        if (monsterFSM.IsPing)
        {
            return true;
        }
        return false;
    }
}
