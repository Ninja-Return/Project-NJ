using FSM_System.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DronStateRoot : FSM_State_Netcode<DronState>
{
    protected DronFSM dronFSM;
    protected Animator anim;
    protected NavMeshAgent nav;
    protected LayerMask playerMask;

    public DronStateRoot(DronFSM controller) : base(controller)
    {
        this.dronFSM = controller;
        anim = dronFSM.anim;
        nav = dronFSM.nav;
        playerMask = dronFSM.playerMask;
    }
}