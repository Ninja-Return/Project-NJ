using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public class ChaseState : MonsterStateRoot
{
    private float radius;
    private float speed;

    public ChaseState(MonsterFSM controller, float radius, float speed) : base(controller)
    {
        this.radius = radius;
        this.speed = speed;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        nav.speed = speed;
    }

    protected override void UpdateState()
    {
        //if (!IsServer) return;

        Vector3 playerPos = monsterFSM.targetPlayer.transform.position;
        nav.SetDestination(playerPos);

        Collider player = monsterFSM.ViewingPlayer(radius);

        if (player != null)
        {
            monsterFSM.targetPlayer = player;
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
