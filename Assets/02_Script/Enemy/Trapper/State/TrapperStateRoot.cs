using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;
using UnityEngine.AI;

public class TrapperStateRoot : FSM_State_Netcode<TrapperState>
{
    protected TrapperFSM trapperFSM;
    protected MonsterAnimation trapperAnim;
    protected NavMeshAgent nav;

    public TrapperStateRoot(TrapperFSM controller) : base(controller)
    {
        trapperFSM = controller;
        trapperAnim = trapperFSM.trapperAnim;
        nav = trapperFSM.nav;
    }
}
