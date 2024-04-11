using FSM_System.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DronTransitionRoot : FSM_Transition_Netcode<DronState>
{
    protected DronFSM dronFSM;

    protected NavMeshAgent nav;
    protected Transform monsterTrs;
    protected Transform headTrs;
    protected float angle;
    protected LayerMask playerMask;

    public DronTransitionRoot(DronFSM controller, DronState nextState) : base(controller, nextState)
    {
        dronFSM = controller;

        nav = dronFSM.nav;
        monsterTrs = dronFSM.transform;
        headTrs = dronFSM.headTrs;
        angle = dronFSM.angle;
        playerMask = dronFSM.playerMask;
    }

    protected override bool CheckTransition()
    {
        return false;
    }
}
