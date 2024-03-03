using FSM_System;
using FSM_System.Netcode;
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


public class EnemyFSM : FSM_Controller_Netcode<EnemyState>
{
    private void Start()
    {
        InitializeStates();
        ChangeState(EnemyState.Patrol); // ���� �ÿ��� Patrol ���·� ����
    }

    private void InitializeStates()
    {
        //AddState(new IdleState(this), EnemyState.Idle);
        //AddState(new PatrolState(this), EnemyState.Patrol);
        // Add other states similarly
    }
}
