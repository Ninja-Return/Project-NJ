using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;

public class PingTransition : FSM_Transition<MonsterState>
{
    

    public PingTransition(FSM_Controller<MonsterState> controller, MonsterState nextState, Vector3 pos) : base(controller, nextState)
    {
        
    }

    protected override bool CheckTransition()
    {
        return false;
    }
}
