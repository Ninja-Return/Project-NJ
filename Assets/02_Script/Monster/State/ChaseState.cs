using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public class ChaseState : FSM_State_Netcode<MonsterState>
{
    public ChaseState(FSM_Controller_Netcode<MonsterState> controller) : base(controller)
    {

    }

    protected override void EnterState()
    {
        base.EnterState();
    }

    protected override void UpdateState()
    {
        base.UpdateState();
    }

    protected override void ExitState()
    {
        base.ExitState();
    }
}
