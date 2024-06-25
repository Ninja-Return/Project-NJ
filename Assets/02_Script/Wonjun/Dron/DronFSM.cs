using Cinemachine;
using FSM_System.Netcode;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using DG.Tweening;
using Unity.Services.Lobbies.Models;
using Unity.Mathematics;

public enum DronState
{
    Idle,
    Patrol,
    Chase,
    Kill,
    Dead,
    Zoom,
}

public class DronFSM : FSM_Controller_Netcode<DronState>, IMachineInterface
{
    public UnityEngine.AI.NavMeshAgent nav;
    public Transform headTrs;
    public CinemachineVirtualCamera jsVcamTrs;
    public float angle;
    public Vector3 lookVec;
    public LayerMask playerMask;
    private CinemachineBasicMultiChannelPerlin noise;

    public DronState nowState;

    [HideInInspector] public Vector3 pingPos;
    [HideInInspector] public PlayerController targetPlayer;
    [HideInInspector] public bool IsDead { get; private set; }
    [HideInInspector] public bool IsKill;

    [Header("Values")]
    [SerializeField] private float moveRadius;
    [SerializeField] private float chaseInRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float killRadius;
    [SerializeField] private float workSpeed;
    [SerializeField] public bool zoom = false;
    [SerializeField] private float runSpeed;
    [SerializeField] private Light dronLight;
    [SerializeField] private float zoomRange;
    [SerializeField] private ParticleSystem Spark;
    [SerializeField] float shakeAmount;
    [SerializeField] float shakeTime = 1.0f;
    [SerializeField] private LayerMask obstacleMask;

    private void Start()
    {
        if (!IsHost)
        {
            nav.enabled = false;
        }
        noise = jsVcamTrs.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (!IsServer) return;

        base.Awake();

        InitializeStates();
        ChangeState(DronState.Idle);
    }

    private void InitializeStates()
    {
        DronIdleState dronIdleState = new DronIdleState(this);
        DronPatrolState dronPatrolState = new DronPatrolState(this, moveRadius, workSpeed, Spark);
        DronChaseState dronChaseState = new DronChaseState(this, chaseRadius, runSpeed);
        DronKillState dronKillState = new DronKillState(this);
        DronZoomState dronZoomState = new DronZoomState(this, zoomRange, dronLight, 3f);
        DronDeathState dronDeathState = new DronDeathState(this);

        DronMoveTransition dronMoveTransition = new DronMoveTransition(this, DronState.Zoom);
        DronInPlayerTransition dronChasePlayerTransition = new DronInPlayerTransition(this, DronState.Chase, chaseInRadius);
        DronCatchPlayerTransition dronCatchPlayerTransition = new DronCatchPlayerTransition(this, DronState.Kill, killRadius);
        DronDieTransition dronDieTransition = new DronDieTransition(this, DronState.Dead);
        DronZoomInTransition dronZoomInTransition = new DronZoomInTransition(this, DronState.Chase);

        dronPatrolState.AddTransition(dronMoveTransition);

        dronIdleState.AddTransition(dronChasePlayerTransition);
        dronPatrolState.AddTransition(dronChasePlayerTransition);
        dronZoomState.AddTransition(dronZoomInTransition);

        dronChaseState.AddTransition(dronCatchPlayerTransition);

        dronIdleState.AddTransition(dronDieTransition);
        dronPatrolState.AddTransition(dronDieTransition);
        dronChaseState.AddTransition(dronDieTransition);
        dronKillState.AddTransition(dronDieTransition);
        dronZoomState.AddTransition(dronDieTransition);

        AddState(dronIdleState, DronState.Idle);
        AddState(dronPatrolState, DronState.Patrol);
        AddState(dronChaseState, DronState.Chase);
        AddState(dronKillState, DronState.Kill);
        AddState(dronDeathState, DronState.Dead);
        AddState(dronZoomState, DronState.Zoom);
    }

    private void FixedUpdate()
    {
        if (!IsServer) return;
        if (targetPlayer == null) return;

        Vector3 targetPlayerPos = targetPlayer.transform.position;
        Vector3 playerVec = (targetPlayerPos - transform.position).normalized;
        lookVec = playerVec;
    }

    private bool RayObstacle(Vector3 pos, Vector3 lookVec, float destance)
    {
        return Physics.Raycast(pos, lookVec, destance, obstacleMask);
    }

    public Collider CirclePlayer(float radius)
    {
        Vector3 pos = headTrs.position;

        Collider[] allPlayers = Physics.OverlapSphere(pos, radius, playerMask);
        if (allPlayers.Length == 0) return null;

        float minDistance = float.MaxValue;
        Collider targetPlayer = null;
        foreach (Collider player in allPlayers)
        {
            if (Vector3.Distance(player.transform.position, pos) < minDistance)
            {
                targetPlayer = player;
            }
        }

        Vector3 targetPos = targetPlayer.transform.position;
        Debug.DrawLine(pos, targetPos, Color.red);

        return targetPlayer;
    }

    public Collider ViewingPlayer(float radius, float angles)
    {
        List<Collider> players = new List<Collider>();

        Vector3 pos = headTrs.position;
        Vector3 eulerAngles = headTrs.eulerAngles;

        float lookingAngle = eulerAngles.y;
        Vector3 rightDir = AngleToDirX(lookingAngle + angle * 0.5f, angles);
        Vector3 leftDir = AngleToDirX(lookingAngle - angle * 0.5f, angles);
        Vector3 upDir = AngleToDirY(lookingAngle, true, angles);
        Vector3 downDir = AngleToDirY(lookingAngle, false, angles);
        Vector3 lookDir = AngleToDirX(lookingAngle, angles);

#if UNITY_EDITOR
        Debug.DrawRay(pos, rightDir * radius, Color.blue);
        Debug.DrawRay(pos, leftDir * radius, Color.blue);
        Debug.DrawRay(pos, upDir * radius, Color.blue);
        Debug.DrawRay(pos, downDir * radius, Color.blue);
        Debug.DrawRay(pos, lookDir * radius, Color.cyan);
#endif

        Collider[] allPlayers = Physics.OverlapSphere(pos, radius, playerMask);
        if (allPlayers.Length == 0)
        {
            return null;
        }

        foreach (Collider player in allPlayers)
        {
            Vector3 targetPos = player.transform.position;
            Vector3 targetDir = (targetPos - pos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
            float playerDistance = Vector3.Distance(player.transform.position, pos);

            if (targetAngle <= angle * 0.5f && !RayObstacle(pos, targetDir, playerDistance))
            {
                players.Add(player);
                Debug.DrawLine(pos, targetPos, Color.red);
            }
        }

        if (players.Count == 0) return null;

        float minDistance = float.MaxValue;
        Collider targetPlayer = null;
        foreach (Collider player in players)
        {
            if (Vector3.Distance(player.transform.position, pos) < minDistance)
            {
                targetPlayer = player;
            }
        }

        return targetPlayer;
    }

    private Vector3 AngleToDirX(float angle, float xRotation)
    {
        float radian = angle * Mathf.Deg2Rad;
        float y = -Mathf.Sin(xRotation * Mathf.Deg2Rad);
        return new Vector3(Mathf.Sin(radian), y, Mathf.Cos(radian));
    }

    private Vector3 AngleToDirY(float angle1, bool isUp, float xRotation)
    {
        float radian1 = angle1 * Mathf.Deg2Rad;
        float radian2 = (angle * 0.5f) * Mathf.Deg2Rad;
        float y = -Mathf.Sin(xRotation * Mathf.Deg2Rad);
        Vector3 angleVec = isUp == true ? new Vector3(0f, Mathf.Sin(radian2), 0f) : new Vector3(0f, -Mathf.Sin(radian2), 0f);
        return new Vector3(Mathf.Sin(radian1), y, Mathf.Cos(radian1)) + angleVec;
    }

    // 드론이 플레이어를 감지하면 멈추게 함
    public void Stun(float time)
    {
        if (targetPlayer != null)
        {
            Debug.Log("스턴 들어옴");
            StunPlayerClientRPC(targetPlayer.OwnerClientId, time, targetPlayer.GetComponent<NetworkObject>().NetworkObjectId);
        }
    }

    [ClientRpc]
    private void StunPlayerClientRPC(ulong clientId, float stunTime, ulong targetPlayerId)
    {
        if (clientId != NetworkManager.LocalClientId) return;

        // 현재 플레이어의 움직임을 비활성화합니다.
        NetworkObject targetNetworkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[targetPlayerId];
        var player = targetNetworkObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.DisableMovement(stunTime);
            Debug.Log("플레이어 감지해서 보냄");
        }
        else
        {
            Debug.LogError("플레이어 컨트롤러를 찾을 수 없습니다.");
        }
    }


    public void JumpScare()
    {
        var player = targetPlayer.GetComponent<PlayerController>();
        JumpScareClientRPC(player.OwnerClientId);
    }

    [ClientRpc]
    private void JumpScareClientRPC(ulong clientId)
    {
        if (clientId != NetworkManager.LocalClientId) return;
        jsVcamTrs.Priority = 500;
        headTrs.localRotation = Quaternion.Euler(30f, 0f, 0f);
        StartCoroutine(Shake(shakeAmount, shakeTime));
    }

    IEnumerator Shake(float ShakeAmount, float ShakeTime)
    {
        noise.m_AmplitudeGain = ShakeAmount;
        noise.m_FrequencyGain = ShakeAmount;

        yield return new WaitForSeconds(ShakeTime);

        noise.m_FrequencyGain = 0f;
        noise.m_AmplitudeGain = 0f;

        KillPlayerServerRPC();

        headTrs.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    [ServerRpc(RequireOwnership = false)]
    public void KillPlayerServerRPC()
    {
        if (!IsServer) return;

        var player = targetPlayer.GetComponent<PlayerController>();

        PlayerManager.Instance.PlayerDie(EnumList.DeadType.Dron, player.OwnerClientId);
        IsKill = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeathServerRpc()
    {
        IsDead = true;
    }

    protected override void Update()
    {
        if (!IsServer) return;

        nowState = currentState;

        base.Update();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, moveRadius);
        Gizmos.DrawWireSphere(transform.position, killRadius);
    }
#endif
}
