using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronStunState : DronStateRoot
{
    private float stunTime;

    public DronStunState(DronFSM fsm, float stunTime) : base(fsm)
    {
        this.stunTime = stunTime;
    }

    protected override void EnterState()
    {
        dronFSM.nav.isStopped = true; // 드론 움직임 멈추기
        dronFSM.StartCoroutine(StunCoroutine());
    }

    protected override void ExitState()
    {
        dronFSM.nav.isStopped = false; // 드론 움직임 다시 시작
    }

    private IEnumerator StunCoroutine()
    {
        yield return new WaitForSeconds(stunTime);
        dronFSM.ChangeState(DronState.Idle); // 스턴 시간이 끝나면 Idle 상태로 변경
    }
}

