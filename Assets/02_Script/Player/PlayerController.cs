using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;
using Cinemachine;
using Unity.VisualScripting;
using DG.Tweening;
using Unity.Netcode;

public enum EnumPlayerState
{

    Idle, //시스템적으로 이동 불가 상태일때
    Move, //이동 가능 상태일때

}

public class PlayerController : FSM_Controller_Netcode<EnumPlayerState>
{

    [SerializeField] private EnumPlayerState startState;
    [SerializeField] private bool debug;

    [field:SerializeField] public PlayerDataSO Data { get; private set; }
    [field:SerializeField] public PlayerInputDataSO Input { get; private set; }

    private CinemachineVirtualCamera cvcam;
    private Canvas interactionCanvas;

    protected override void Awake()
    {
        
        base.Awake();

        cvcam = GetComponentInChildren<CinemachineVirtualCamera>();
        interactionCanvas = GetComponentInChildren<Canvas>();

    }

    private void Start()
    {

        cvcam.Priority = IsOwner || debug ? 10 : 0;
        interactionCanvas.gameObject.SetActive(IsOwner || debug);

        if(!IsOwner && !debug) return;

        if (!debug)
        {

            JoinChannel();

        }

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

        var interaction = new PlayerInteraction(this);
        AddState(interaction, EnumPlayerState.Move);

        ChangeState(startState);

    }

    private async void JoinChannel()
    {

        await NetworkController.Instance.vivox.Join3DChannel();
        StartCoroutine(Update3DPosCo());

    }

    protected override void Update()
    {

        if (!IsOwner && !debug) return;

        if(GameManager.Instance != null)
        {

            if (!GameManager.Instance.PlayerMoveable) return;

        }

        base.Update();

    }

    private IEnumerator Update3DPosCo()
    {

        var sec = new WaitForSecondsRealtime(0.1f);

        while (true)
        {

            NetworkController.Instance.vivox.UpdateChannelPos(gameObject);

            yield return sec;

        }

    }

    public void PlayerDieOwnerRPC(ClientRpcParams clientParams = default)
    {



    }

}