using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;
using UnityEngine.AI;

public class TrapperDieTransition : FSM_Transition_Netcode<TrapperState>
{
    private TrapperFSM trapperFSM;

    public TrapperDieTransition(TrapperFSM controller, TrapperState nextState) : base(controller, nextState)
    {
        trapperFSM = controller;
    }

    protected override bool CheckTransition()
    {
        if (trapperFSM.IsDead)
        {
            return true;
        }
        return false;
    }
}
