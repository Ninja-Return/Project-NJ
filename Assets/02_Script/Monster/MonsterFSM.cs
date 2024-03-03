using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FSM_System.Netcode;

public enum MonsterState
{
    Idle,
    Patrol,
    Ping,
    Chase,
    Kill,
    Dead
}

public class MonsterFSM : FSM_Controller_Netcode<MonsterState>
{
    public Vector3 pingPos;

    private NavMeshAgent nav;

    protected override void Awake()
    {
        base.Awake();

        InitializeStates();
        ChangeState(MonsterState.Idle);
    }

    private void InitializeStates()
    {
        IdleState idleState = new IdleState(this);
        PatrolState patrolState = new PatrolState(this);
        PingState pingState = new PingState(this);

        //patrolState.AddTransition(new MoveTransition(this, MonsterState.Idle, nav));

        AddState(idleState, MonsterState.Idle);
        AddState(patrolState, MonsterState.Patrol);
        AddState(pingState, MonsterState.Ping);
    }

    public void SetPingPos(Vector3 pos)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas)) 
        {
            pingPos = hit.position;

            if (currentState == MonsterState.Idle || currentState == MonsterState.Patrol)
            {
                ChangeState(MonsterState.Ping);
            }
        }
    }
}
