using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public class DeathState : FSM_State_Netcode<MonsterState>
{
    public DeathState(FSM_Controller_Netcode<MonsterState> controller) : base(controller)
    {

    }

    protected override void EnterState()
    {
        //죽는 애니메이션
        //리썰처럼 시체 냅두는게 좋을듯?
    }

    protected override void UpdateState()
    {
        
    }

    protected override void ExitState()
    {
        
    }
}
