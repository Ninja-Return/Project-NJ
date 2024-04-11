using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronDeathState : DronStateRoot
{
    public DronDeathState(DronFSM controller) : base(controller)
    {

    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        dronFSM.SetAnimation("Death", true);

        //�״� �ִϸ��̼�
        //����ó�� ��ü ���δ°� ������?
    }

    protected override void UpdateState()
    {

    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        dronFSM.SetAnimation("Death", false);
    }
}
