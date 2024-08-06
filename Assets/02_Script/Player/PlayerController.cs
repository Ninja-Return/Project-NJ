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
    Idle, // 시스템적으로 이동 불가 상태일 때
    Move, // 이동 가능 상태일 때
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
    public int onEmotionkeyNumber;
    public Vector3 targetCameraPosition;
    public float changeTime = 1f;
    public Vector3 originalCameraPosition;
    public Slider SensitivitySlider;
    public Rigidbody playerRigidbody;

    public NetworkVariable<float> psychosisValue { get; private set; }
    = new(writePerm: NetworkVariableWritePermission.Owner);
    public NetworkVariable<Vector2> Velocity { get; private set; }
    = new(writePerm: NetworkVariableWritePermission.Owner);

    // 사용되지 않음
    public bool IsMeeting { get; set; }

    public CinemachineVirtualCamera watchCam { get; private set; }

    private bool isMovementDisabled = false; // 움직임 비활성화 여부

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

        if (isMovementDisabled)
        {
            playerRigidbody.velocity = Vector3.zero; // 플레이어를 완전히 멈추게 함
            return;
        }

        // 기존 이동 관련 코드
        Velocity.Value = playerRigidbody.velocity;

        base.Update();
    }

    public async void Stop()
    {
        await HostSingle.Instance.GameManager.ShutdownAsync();
        SceneManager.LoadScene(SceneList.LobbySelectScene);
    }

    private IEnumerator Update3DPosCo()
    {
        var sec = new WaitForSecondsRealtime(0.05f);

        while (true)
        {
            if (gameObject == null) yield break;

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

        if (IsOwner && Inventory.Instance != null)
        {
            Input.OnInventoryActivePress -= HandleInvenActive;
        }

        if (IsOwner && Input != null)
        {
            Input.Dispose();
            Destroy(Input);
        }

        if (IsServer && PlayerManager.Instance != null)
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

            if (disAbleInven && Inventory.Instance.isShow)
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

    public void CaughtTrap(float time)
    {
        CaughtTrapClientRpc(time, OwnerClientId.GetRPCParams());
    }

    [ClientRpc]
    private void CaughtTrapClientRpc(float time, ClientRpcParams param)
    {
        DisableMovement(time); // 플레이어 움직임 비활성화
    }

    private IEnumerator SpeedCo(float speed, float time)
    {
        Data.MoveSpeed.AddMod(speed);
        yield return new WaitForSeconds(time);
        Data.MoveSpeed.RemoveMod(speed);
    }

    private IEnumerator PlayerStopCor(float time)
    {
        float slowSpeed = 0.5f;
        float moveSpeed = Data.MoveSpeed.DefaultValue;
        float jumpSpeed = Data.JumpPower.DefaultValue;

        Data.MoveSpeed.SetValue(slowSpeed);
        Data.JumpPower.SetValue(slowSpeed);

        yield return new WaitForSeconds(time);

        Data.MoveSpeed.SetValue(moveSpeed);
        Data.JumpPower.SetValue(jumpSpeed);
    }

    public void DisableMovement(float duration)
    {
        Debug.Log("플레이어 멈추기 시작");
        StartCoroutine(DisableMovementCoroutine(duration));
    }

    private IEnumerator DisableMovementCoroutine(float duration)
    {
        isMovementDisabled = true;
        playerRigidbody.velocity = Vector3.zero; // 플레이어를 완전히 멈추게 함
        yield return new WaitForSeconds(duration);
        isMovementDisabled = false;
    }
}
