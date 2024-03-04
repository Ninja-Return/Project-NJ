using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public class ChaseState : MonsterStateRoot
{
    private float radius;

    public ChaseState(MonsterFSM controller, float radius) : base(controller)
    {
        this.radius = radius;
    }

    protected override void EnterState()
    {
        
    }

    protected override void UpdateState()
    {
        //if (!IsServer) return;

        Vector3 playerPos = monsterFSM.targetPlayer.transform.position;
        nav.SetDestination(playerPos);

        if (monsterFSM.ViewingPlayer(radius) != null)
        {
            monsterFSM.targetPlayer = monsterFSM.ViewingPlayer(radius);
        }
        else
        {
            monsterFSM.ChangeState(MonsterState.Idle);
        }
    }

    protected override void ExitState()
    {
        
    }
}
