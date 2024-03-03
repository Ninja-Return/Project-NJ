using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;
using UnityEngine.AI;

public class MoveTransition : FSM_Transition<MonsterState>
{
    private NavMeshAgent nav;

    public MoveTransition(FSM_Controller<MonsterState> controller, MonsterState nextState, NavMeshAgent nav) : base(controller, nextState)
    {
        this.nav = nav;
    }

    protected override bool CheckTransition()
    {
        if (Vector3.Distance(nav.destination, controller.transform.position) <= 2f)
        {
            return true;
        }
        return false;
    }
}
