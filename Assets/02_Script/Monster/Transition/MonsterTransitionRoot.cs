using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FSM_System.Netcode;

public class MonsterTransitionRoot : FSM_Transition_Netcode<MonsterState>
{
    protected MonsterFSM monsterFSM;

    protected NavMeshAgent nav;
    protected Transform headTrs;
    protected float angle;
    protected LayerMask playerMask;

    public MonsterTransitionRoot(MonsterFSM controller, MonsterState nextState) : base(controller, nextState)
    {
        monsterFSM = controller;

        nav = controller.nav;
        headTrs = monsterFSM.headTrs;
        angle = monsterFSM.angle;
        playerMask = monsterFSM.playerMask;
    }

    protected override bool CheckTransition()
    {
        return false;
    }
}
