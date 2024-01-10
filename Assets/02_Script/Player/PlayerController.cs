using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public enum EnumPlayerState
{

    Idle, //�ý��������� �̵� �Ұ� �����϶�
    Move, //�̵� ���� �����϶�

}

public class PlayerController : FSM_Controller_Netcode<EnumPlayerState>
{
    
    [field:SerializeField] public PlayerDataSO Data { get; private set; }

}