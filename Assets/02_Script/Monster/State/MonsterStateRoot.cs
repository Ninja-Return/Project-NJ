using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;
using UnityEngine.AI;

public class MonsterStateRoot : FSM_State_Netcode<MonsterState>
{
    protected MonsterFSM monsterFSM;
    protected Animator anim;
    protected NavMeshAgent nav;

    public MonsterStateRoot(MonsterFSM controller) : base(controller)
    {
        this.monsterFSM = controller;
        anim = monsterFSM.GetComponent<Animator>();
        nav = monsterFSM.GetComponent<NavMeshAgent>();
    }
}
