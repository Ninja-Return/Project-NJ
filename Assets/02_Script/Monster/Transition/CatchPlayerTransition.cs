using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public class CatchPlayerTransition : MonsterTransitionRoot
{
    private float radius;

    public CatchPlayerTransition(MonsterFSM controller, MonsterState nextState, float radius) : base(controller, nextState) 
    {
        this.radius = radius;
    }

    protected override bool CheckTransition()
    {
        if (nav.pathPending) return false;
        
        if (monsterFSM.targetPlayer == null) return false;
        
        if (monsterFSM.CirclePlayer(radius) != null)
        {
            return true;
        }

        return false;
    }
}
