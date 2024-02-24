using FSM_System;
using UnityEngine;

public class PatrolState : FSM_State<EnemyState>
{
    private float patrolTime = 2f; // ���� �ð�
    private float idleTime = 1f;   // ��� �ð�
    private Vector3 patrolDestination; // ���� ������

    private float timer = 0f; // �ð� ������ Ÿ�̸�

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
        // ������ ��ġ�� ���� �������� ����
        patrolDestination = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
    }

    private void MoveToDestination()
    {
        transform.Translate(patrolDestination * Time.deltaTime * 5f);
        // ���� �������� �̵�
        // ���⿡�� ���� �̵� ������ �����մϴ�.
    }
}

