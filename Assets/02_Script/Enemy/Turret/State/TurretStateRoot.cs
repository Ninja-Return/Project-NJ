using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;
using UnityEngine.AI;

public class TurretStateRoot : FSM_State_Netcode<TurretState>
{
    protected TurretFSM turretFSM;

    public TurretStateRoot(TurretFSM controller) : base(controller)
    {
        turretFSM = controller;
    }
}
