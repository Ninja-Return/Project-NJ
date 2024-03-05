using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;
using UnityEngine.AI;

public class MoveTransition : MonsterTransitionRoot
{
    public MoveTransition(MonsterFSM controller, MonsterState nextState) : base(controller, nextState) { }

    protected override bool CheckTransition()
    {
        if (Vector3.Distance(nav.destination, controller.transform.position) <= 2f)
        {
            return true;
        }
        return false;
    }
}
