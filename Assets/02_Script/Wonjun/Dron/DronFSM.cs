using Cinemachine;
using FSM_System.Netcode;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public enum DronState
{
    Idle,
    Patrol,
    Chase,
    Kill,
    Dead,
    Zoom,
    Stun,
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
    [SerializeField] private float runSpeed;
    [SerializeField] private float raderTime;
    [SerializeField] private float chaseTime;
    [SerializeField] private float zoomRange;
    [SerializeField] public bool zoom = false;

    [Header("Obj")]
    [SerializeField] private Light dronLight;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private ParticleSystem Spark;
    [SerializeField] private LineRenderer laserLine;

    [Header("Other")]
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
        DronChaseState dronChaseState = new DronChaseState(this, chaseRadius, runSpeed, raderTime, chaseTime, dronLight, timeText);
        DronKillState dronKillState = new DronKillState(this);
        DronZoomState dronZoomState = new DronZoomState(this, zoomRange, dronLight, 3f);
        DronDeathState dronDeathState = new DronDeathState(this);
        DronStunState dronStunState = new DronStunState(this, 10f); // 10초 동안 스턴 상태 유지

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
        AddState(dronStunState, DronState.Stun); // 스턴 상태 추가
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
        Debug.DrawRay(pos, lookDir * radius, Color.green);
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
    public void PlayerStun(float stunTime)
    {
        if (targetPlayer != null)
        {
            Debug.Log("스턴 들어옴");
            StunPlayerClientRPC(targetPlayer.OwnerClientId, stunTime, targetPlayer.GetComponent<NetworkObject>().NetworkObjectId);
        }
    }

    // 드론 정지
    public void Stun(float time)
    {
        if (!IsServer || IsDead) return;

        ChangeState(DronState.Stun); // 스턴 상태로 전이
    }

    [ClientRpc]
    private void StunPlayerClientRPC(ulong clientId, float stunTime, ulong targetPlayerId)
    {
        if (clientId != NetworkManager.LocalClientId) return;

        NetworkObject targetNetworkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[targetPlayerId];
        var player = targetNetworkObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.DisableMovement(stunTime);
            StartCoroutine(LaserBeam(headTrs, stunTime, player.transform));
            Debug.Log("플레이어 컨트롤러 찾았고 스턴으로 넘김");
        }
        else
        {
            Debug.LogError("스턴을 하려는데 플레이어 컨트롤러 컴포넌트 못찾음");
        }
    }

    private IEnumerator LaserBeam(Transform headTrs, float time, Transform playerTrs)
    {
        if (playerTrs != null)
        {
            laserLine.enabled = true; // 레이저 활성화
            Debug.Log(headTrs.position);
            Debug.Log(playerTrs.position);

            Vector3 initialStartPosition = headTrs.position + new Vector3(0, 0.5f, 0); // 레이저 초기 시작 위치
            Vector3 targetPosition = playerTrs.position + new Vector3(0, 0.5f, 0); // 레이저 목표 위치

            laserLine.SetPosition(0, initialStartPosition);

            float elapsedTime = 0f;
            float duration = 0.5f; // 레이저 시작 위치가 목표 위치로 이동하는 시간
            float speed = Vector3.Distance(initialStartPosition, targetPosition) / 10; // 레이저의 이동 속도

            // 레이저 끝 위치를 목표 위치로 이동
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float fraction = elapsedTime / duration;
                Vector3 currentEndPosition = Vector3.Lerp(initialStartPosition, targetPosition , fraction);

                laserLine.SetPosition(1, currentEndPosition );

                yield return null; 
            }

            // 레이저 끝 위치가 목표 위치에 도달한 후, 시작 위치를 이동
            elapsedTime = 0f;
            Vector3 currentStartPosition = initialStartPosition;

            while (elapsedTime < speed)
            {
                elapsedTime += Time.deltaTime;
                float fraction = elapsedTime / speed;
                currentStartPosition = Vector3.Lerp(initialStartPosition, targetPosition, fraction);

                laserLine.SetPosition(0, currentStartPosition);
                laserLine.SetPosition(1, targetPosition );

                yield return null; 
            }

            // 최종 위치 설정
            laserLine.SetPosition(0, targetPosition);
            laserLine.SetPosition(1, targetPosition);

            yield return new WaitForSeconds(time);

            laserLine.enabled = false; // 레이저 비활성화
        }
        else
        {
            Debug.LogError("플레이어를 찾을 수 없음요.");
            yield break;
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, moveRadius);
        Gizmos.DrawWireSphere(transform.position, killRadius);
    }
#endif
}
