using FSM_System;
using UnityEngine;

public class IdleState : FSM_State<EnemyState>
{
    private float idleDuration = 1f; // ��� �ð�
    private float timer = 0f; // �ð� ������ Ÿ�̸�

    public IdleState(FSM_Controller<EnemyState> controller) : base(controller) { }

    protected override void EnterState()
    {
        Debug.Log("Entering Idle State");
    }

    protected override void UpdateState()
    {

        // �ð��� ������ �ٽ� Patrol ���·� ��ȯ
        timer += Time.deltaTime;
        if (timer >= idleDuration)
        {
            timer = 0;
            controller.ChangeState(EnemyState.Patrol);
        }
    }

    protected override void ExitState()
    {
        Debug.Log("Exiting Idle State");
    }
}
