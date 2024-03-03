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
