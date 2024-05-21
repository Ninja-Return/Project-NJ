using FSM_System.Netcode;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LigthMonsterStateType
{

    Patrol,
    Chase,
    RunAway

}

public abstract class LightMonsterBaseState : FSM_State_Netcode<LigthMonsterStateType>
{

    protected new LightMonsterFSM controller;

    protected LightMonsterBaseState(FSM_Controller_Netcode<LigthMonsterStateType> controller) : base(controller)
    {

        this.controller = controller as LightMonsterFSM;

    }

}

public class LightMonsterFSM : FSM_Controller_Netcode<LigthMonsterStateType>, ILightCastable
{

    public event Action OnCastedEvent;

    public void Casting()
    {

        OnCastedEvent?.Invoke();

    }

}
