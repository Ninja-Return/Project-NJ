using FSM_System;
using UnityEngine;

public class PatrolToChaseTransition : FSM_Transition<EnemyState>
{
    public PatrolToChaseTransition(FSM_Controller<EnemyState> controller, EnemyState nextState) : base(controller, nextState) { }

    protected override bool CheckTransition()
    {
        // Check if the condition for transitioning from patrol to chase is met
        // For example, check if an enemy is within detection range
        /*if (*//* Condition for transitioning *//*)
        {
            return true;
        }*/
        return false;
    }
}
