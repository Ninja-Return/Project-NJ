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
        dronFSM.nav.isStopped = true; // ��� ������ ���߱�
        dronFSM.StartCoroutine(StunCoroutine());
    }

    protected override void ExitState()
    {
        dronFSM.nav.isStopped = false; // ��� ������ �ٽ� ����
    }

    private IEnumerator StunCoroutine()
    {
        yield return new WaitForSeconds(stunTime);
        dronFSM.ChangeState(DronState.Idle); // ���� �ð��� ������ Idle ���·� ����
    }
}

