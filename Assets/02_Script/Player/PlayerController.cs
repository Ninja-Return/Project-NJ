using System.Collections;
using UnityEngine;
using FSM_System.Netcode;
using Cinemachine;
using System;
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

    private GameObject meetingObject;
    public CinemachineVirtualCamera cvcam;
    private Canvas interactionCanvas;
    private bool isActive = true;
    public bool isSittingDown = false; // 현재 앉아 있는지 여부
    public Vector3 targetCameraPosition;
    public float changeTime = 1f;
    public Vector3 originalCameraPosition;

    public bool IsMeeting { get; set; }

    public CinemachineVirtualCamera watchCam { get; private set; }

    protected override void Awake()
    {
        
        base.Awake();

        cvcam = transform.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        watchCam = cvcam.transform.Find("PlayerWatchingCam").GetComponent<CinemachineVirtualCamera>();

        interactionCanvas = GetComponentInChildren<Canvas>();
        meetingObject = GameObject.Find("MeetingObject");

    }

    private void Start()
    {

        cvcam.Priority = IsOwner || debug ? 10 : 0;

        if (!IsOwner)
        {

            Destroy(cvcam);

        }

        interactionCanvas.gameObject.SetActive(IsOwner || debug);

        if(!IsOwner && !debug) return;

        if (!debug)
        {

            JoinChannel();

        }

        Input = Input.Init();
        Data = Instantiate(Data);

        if(PlayerManager.Instance != null)
        {

            PlayerManager.Instance.SetLocalPlayer(this);

        }

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

        var sitDown = new PlayerSitDown(this);
        AddState(sitDown, EnumPlayerState.Move);

        ChangeState(startState);

        Input.OnInventoryKeyPress += HandleInvenActive;

    }

    private void HandleInvenActive()
    {

        Inventory.Instance.SetActiveInventoryUI();

    }

    private async void JoinChannel()
    {

        //await NetworkController.Instance.vivox.Join3DChannel();
        StartCoroutine(Update3DPosCo());

    }

    protected override void Update()
    {

        if (!IsOwner && !debug) return;

        

        base.Update();

#if UNITY_EDITOR

        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {

            HostSingle.Instance.GameManager.ShutdownAsync();

        }

#endif

        //if (UnityEngine.Input.GetKeyDown(KeyCode.K))
        //{
        //
        //    GameManager.Instance.PlayerDie(EnumList.DeadType.Mafia, OwnerClientId);
        //
        //}

    }

    private IEnumerator Update3DPosCo()
    {

        var sec = new WaitForSecondsRealtime(0.05f);

        while (true)
        {

            if(gameObject == null) yield break;

            if (IsMeeting)
            {

                NetworkController.Instance.vivox.UpdateChannelPos(meetingObject);

            }
            else
            {

                NetworkController.Instance.vivox.UpdateChannelPos(gameObject);

            }


            yield return sec;

        }

    }

    public override void OnDestroy()
    {

        base.OnDestroy();

        if(IsOwner && Inventory.Instance != null)
        {

            Input.OnInventoryKeyPress -= HandleInvenActive;

        }

    }

    [ClientRpc]
    public void SetMafiaClientRPC(ClientRpcParams param)
    {

        var state = new PlayerKillState(this);
        AddState(state, EnumPlayerState.Move);
        ChangeState(EnumPlayerState.Move);

    }

    public void Active(bool active, bool disAbleInven = false)
    {

        if (active)
        {

            ChangeState(EnumPlayerState.Move);

        }
        else
        {

            ChangeState(EnumPlayerState.Idle);

            if(disAbleInven && Inventory.Instance.isShow)
            {

                Inventory.Instance.SetActiveInventoryUI(true);

            }

        }
    }

}