using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;
using UnityEngine.AI;

public class MonsterStateRoot : FSM_State_Netcode<MonsterState>
{
    protected MonsterFSM monsterFSM;
    protected MonsterAnimation monsterAnim;
    protected NavMeshAgent nav;
    protected LayerMask playerMask;

    public MonsterStateRoot(MonsterFSM controller) : base(controller)
    {
        this.monsterFSM = controller;
        monsterAnim = monsterFSM.monsterAnim;
        nav = monsterFSM.nav;
        playerMask = monsterFSM.playerMask;
    }
}
