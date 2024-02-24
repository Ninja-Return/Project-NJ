using FSM_System;
using UnityEngine;

public class PatrolState : FSM_State<EnemyState>
{
    private float patrolTime = 2f; // 순찰 시간
    private float idleTime = 1f;   // 대기 시간
    private Vector3 patrolDestination; // 순찰 목적지

    private float timer = 0f; // 시간 측정용 타이머

    public PatrolState(FSM_Controller<EnemyState> controller) : base(controller) { }

    protected override void EnterState()
    {
        Debug.Log("Entering Patrol State");
        SetRandomPatrolDestination();
    }

    protected override void UpdateState()
    {
        MoveToDestination();

        timer += Time.deltaTime;
        if (timer >= patrolTime)
        {
            timer = 0;
            controller.ChangeState(EnemyState.Idle);
        }
    }

    protected override void ExitState()
    {
        Debug.Log("Exiting Patrol State");
    }

    private void SetRandomPatrolDestination()
    {
        // 랜덤한 위치를 순찰 목적지로 설정
        patrolDestination = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
    }

    private void MoveToDestination()
    {
        transform.Translate(patrolDestination * Time.deltaTime * 5f);
        // 순찰 목적지로 이동
        // 여기에는 적의 이동 로직을 구현합니다.
    }
}

