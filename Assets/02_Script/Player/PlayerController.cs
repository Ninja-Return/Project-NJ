using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public enum EnumPlayerState
{

    Idle, //시스템적으로 이동 불가 상태일때
    Move, //이동 가능 상태일때

}

public class PlayerController : FSM_Controller_Netcode<EnumPlayerState>
{
    
    [field:SerializeField] public PlayerDataSO Data { get; private set; }

}