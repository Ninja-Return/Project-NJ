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

        //죽는 애니메이션
        //리썰처럼 시체 냅두는게 좋을듯?
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
