using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FSM_System.Netcode;

public class PingState : MonsterStateRoot
{
    private float speed;

    public PingState(MonsterFSM controller, float speed) : base(controller) 
    {
        this.speed = speed;
    }

    protected override void EnterState()
    {
        //if (!IsServer) return;

        nav.speed = speed;

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
