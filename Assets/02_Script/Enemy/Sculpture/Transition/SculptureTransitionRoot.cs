using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using FSM_System.Netcode;

public class SculptureTransitionRoot : FSM_Transition_Netcode<SculptureState>
{
    protected SculptureFSM sculptureFSM;
    protected NavMeshAgent nav;

    public SculptureTransitionRoot(SculptureFSM controller, SculptureState nextState) : base(controller, nextState)
    {
        sculptureFSM = controller;
        nav = sculptureFSM.nav;
    }

    protected override bool CheckTransition()
    {
        return false;
    }
}
