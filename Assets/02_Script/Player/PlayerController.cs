using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;
using Cinemachine;

public enum EnumPlayerState
{

    Idle, //�ý��������� �̵� �Ұ� �����϶�
    Move, //�̵� ���� �����϶�

}

public class PlayerController : FSM_Controller_Netcode<EnumPlayerState>
{

    [SerializeField] private EnumPlayerState startState;
    [SerializeField] private bool debug;

    [field:SerializeField] public PlayerDataSO Data { get; private set; }
    [field:SerializeField] public PlayerInputDataSO Input { get; private set; }

    private CinemachineVirtualCamera cvcam;

    protected override void Awake()
    {
        
        base.Awake();

        cvcam = GetComponentInChildren<CinemachineVirtualCamera>();

    }

    private void Start()
    {

        cvcam.Priority = IsOwner ? 10 : 0;

        if(!IsOwner && !debug) return;

        Input = Input.Init();
        Data = Instantiate(Data);

        var defaultState = new PlayerStateRoot(this);
        AddState(defaultState, EnumPlayerState.Idle);

        var move = new PlayerMove(this);
        AddState(move, EnumPlayerState.Move);

        var cameraRotate = new PlayerCameraRotate(this);
        AddState(cameraRotate, EnumPlayerState.Move);

        var jump = new PlayerJump(this);
        AddState(jump, EnumPlayerState.Move);

        ChangeState(startState);

    }

    protected override void Update()
    {

        if (!IsOwner && !debug) return;

        base.Update();

    }

}