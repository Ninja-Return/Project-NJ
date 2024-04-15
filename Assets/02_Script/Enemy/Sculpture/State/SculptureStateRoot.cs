using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using FSM_System.Netcode;

public class SculptureStateRoot : FSM_State_Netcode<SculptureState>
{
    protected SculptureFSM sculptureFSM;
    protected NavMeshAgent nav;

    public SculptureStateRoot(SculptureFSM controller) : base(controller)
    {
        sculptureFSM = controller;
        nav = sculptureFSM.nav;
    }
}
