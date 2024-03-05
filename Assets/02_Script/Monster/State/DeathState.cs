using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public class DeathState : MonsterStateRoot
{
    public DeathState(MonsterFSM controller) : base(controller)
    {

    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Death", true);

        //�״� �ִϸ��̼�
        //����ó�� ��ü ���δ°� ������?
    }

    protected override void UpdateState()
    {
        
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Death", false);
    }
}
