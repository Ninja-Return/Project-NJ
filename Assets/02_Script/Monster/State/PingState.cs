using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FSM_System.Netcode;

public class PingState : MonsterStateRoot
{
    public PingState(MonsterFSM controller) : base(controller) { }

    protected override void EnterState()
    {
        //if (!IsServer) return;

        Vector3 pos = monsterFSM.pingPos;
        nav.SetDestination(pos);
    }

    protected override void UpdateState()
    {
        base.UpdateState();
    }

    protected override void ExitState()
    {
        base.ExitState();
    }
}
