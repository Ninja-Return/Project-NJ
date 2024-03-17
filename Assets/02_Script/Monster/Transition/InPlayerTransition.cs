using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public class InPlayerTransition : MonsterTransitionRoot
{
    private float radius;

    public InPlayerTransition(MonsterFSM controller, MonsterState nextState, float radius) : base(controller, nextState) 
    {
        this.radius = radius;
    }

    protected override bool CheckTransition()
    {
        if (nav.pathPending) return false;

        Collider targetPlayer = monsterFSM.ViewingPlayer(radius);
        if (targetPlayer != null)
        {
            monsterFSM.targetPlayer = targetPlayer;
            return true;
        }

        return false;
    }
}
