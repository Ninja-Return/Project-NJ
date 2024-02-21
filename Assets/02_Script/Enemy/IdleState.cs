using FSM_System;
using UnityEngine;

public class IdleState : FSM_State<EnemyState>
{
    private float idleDuration = 1f; // 대기 시간
    private float timer = 0f; // 시간 측정용 타이머

    public IdleState(FSM_Controller<EnemyState> controller) : base(controller) { }

    protected override void EnterState()
    {
        Debug.Log("Entering Idle State");
    }

    protected override void UpdateState()
    {

        // 시간이 지나면 다시 Patrol 상태로 전환
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
