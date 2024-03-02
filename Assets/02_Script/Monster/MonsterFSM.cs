using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public enum MonsterState
{
    Idle,
    Patrol,
    Ping,
    Chase,
    Kill,
    Dead
}

public class MonsterFSM : FSM_Controller_Netcode<MonsterState>
{
    
}
