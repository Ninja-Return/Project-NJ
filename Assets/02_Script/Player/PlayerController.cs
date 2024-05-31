using Cinemachine;
using FSM_System.Netcode;
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EnumPlayerState
{

    Idle, //시스템적으로 이동 불가 상태일때
    Move, //이동 가능 상태일때
   
}

public class PlayerController : FSM_Controller_Netcode<EnumPlayerState>, ICatchTrapInterface
{
    [SerializeField] private EnumPlayerState startState;
    [SerializeField] private bool debug;

    [field: SerializeField] public PlayerDataSO Data { get; private set; }
    [field: SerializeField] public PlayerInputDataSO Input { get; private set; }

    private GameObject meetingObject;
    private Canvas interactionCanvas;
    public PlayerImpulse Impulse { get; private set; }

    public CinemachineVirtualCamera cvcam { get; private set; }

    public bool isInsideSafetyRoom { get; set; } = true;
    public bool isSittingDown = false; // 현재 앉아 있는지 여부
    public Vector3 targetCameraPosition;
    public float changeTime = 1f;
    public Vector3 originalCameraPosition;
    public Slider SensitivitySlider;
    public Rigidbody playerRigidbody;

    public NetworkVariable<float> psychosisValue { get; private set; }
    = new(writePerm: NetworkVariableWritePermission.Owner);
    public NetworkVariable<Vector2> moveVector { get; private set; }
    = new(writePerm: NetworkVariableWritePermission.Owner);

    //사용되지 않음
    public bool IsMeeting { get; set; }

    public CinemachineVirtualCamera watchCam { get; private set; }

    protected override void Awake()
    {
        
        base.Awake();

        cvcam = transform.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        watchCam = cvcam.transform.Find("PlayerWatchingCam").GetComponent<CinemachineVirtualCamera>();

        interactionCanvas = GetComponentInChildren<Canvas>();
        meetingObject = GameObject.Find("MeetingObject");

        Impulse = GetComponent<PlayerImpulse>();
        playerRigidbody = GetComponent<Rigidbody>();

    }


    private void Start()
    {

        cvcam.Priority = IsOwner || debug ? 10 : 0;

        if (!IsOwner)
        {

            Destroy(cvcam);

        }

        interactionCanvas.gameObject.SetActive(IsOwner || debug);

        if (!IsOwner && !debug) return;

        if (!debug)
        {

            JoinChannel();

        }

        Input = Input.Init();
        Data = Data.Copy();

        if (PlayerManager.Instance != null)
        {

            PlayerManager.Instance.SetLocalPlayer(this);

        }

        Camera.main.fieldOfView = PlayerPrefs.GetFloat("FOV");

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

        Input.OnInventoryActivePress += HandleInvenActive;

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

        //Data.LookSensitive.SetValue(SensitivitySlider.value);
        moveVector.Value = Input.MoveVecter;

        base.Update();

    }

    public async void Stop()
    {

        await HostSingle.Instance.GameManager.ShutdownAsync();

        SceneManager.LoadScene(SceneList.LobbySelectScene);

    }

    public void CaughtTrap(float time) //Server에서 호출
    {
        //점프도 안되게
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

            Input.OnInventoryActivePress -= HandleInvenActive;

        }

        if(IsOwner && Input != null)
        {

            Input.Dispose();
            Destroy(Input);

        }

        if(IsServer && PlayerManager.Instance != null)
        {

            PlayerManager.Instance.PlayerExit(OwnerClientId);

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

        playerRigidbody.velocity = new Vector3(0f, playerRigidbody.velocity.y, 0f);

        if (active)
        {

            ChangeState(EnumPlayerState.Move);

        }
        else
        {

            ChangeState(EnumPlayerState.Idle);
            Input.InitMoveVector();

            if(disAbleInven && Inventory.Instance.isShow)
            {

                Inventory.Instance.SetActiveInventoryUI(true);

            }

        }
    }

    public void PlayImpulse(string name)
    {

        PlayImpulseClientRPC(name);

    }

    [ClientRpc]
    private void PlayImpulseClientRPC(string name)
    {

        Impulse.PlayImpulse(name);

    }

    public void AddSpeed(float value, float time)
    {

        StartCoroutine(SpeedCo(value, time));

    }

    [ClientRpc]
    public void AddSpeedClientRPC(float value, float time)
    {

        AddSpeed(value, time);

    }

    private IEnumerator SpeedCo(float speed, float time) 
    {

        Data.MoveSpeed.AddMod(speed);

        yield return new WaitForSeconds(time);

        Data.MoveSpeed.RemoveMod(speed);

    }

}