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
        if (monsterFSM.targetPlayer == null) return false;

        Vector3 targetPlayerPos = monsterFSM.targetPlayer.transform.position;
        if (Vector3.Distance(monsterTrs.position, targetPlayerPos) <= radius)
        {
            return true;
        }

        return false;
    }
}
