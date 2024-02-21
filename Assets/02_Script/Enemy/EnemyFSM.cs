using FSM_System;
using System;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Dead
}


public class EnemyFSM : FSM_Controller<EnemyState>
{
    private void Start()
    {
        InitializeStates();
        ChangeState(EnemyState.Patrol); // 시작 시에는 Patrol 상태로 설정
    }

    private void InitializeStates()
    {
        AddState(new IdleState(this), EnemyState.Idle);
        AddState(new PatrolState(this), EnemyState.Patrol);
        // Add other states similarly
    }
}
