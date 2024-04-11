using Cinemachine;
using FSM_System.Netcode;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum DronState
{
    Idle,
    Patrol,
    Ping,
    Chase,
    Kill,
    Dead
}


public class DronFSM : FSM_Controller_Netcode<DronState>
{
    public Animator anim;
    public UnityEngine.AI.NavMeshAgent nav;
    public Transform headTrs;
    public CinemachineVirtualCamera jsVcamTrs;
    public float angle;
    public Vector3 lookVec;
    public LayerMask playerMask;

    public DronState nowState;

    [HideInInspector] public Vector3 pingPos;
    [HideInInspector] public Collider targetPlayer;
    [HideInInspector] public bool IsDead { get; private set; }
    [HideInInspector] public bool IsKill;

    [Header("Values")]
    [SerializeField] private float moveRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float killRadius;
    [SerializeField] private float workSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private LayerMask obstacleMask;

    private void Start()
    {
        if (!IsHost)
        {
            nav.enabled = false;
        }

        if (!IsServer) return;

        base.Awake();

        InitializeStates();
        ChangeState(DronState.Idle);
    }

    private void InitializeStates()
    {
        DronIdleState dronIdleState = new DronIdleState(this);
        DronPatrolState dronPatrolState = new DronPatrolState(this, moveRadius, workSpeed);
        DronPingState dronPingState = new DronPingState(this, workSpeed);
        DronChaseState dronChaseState = new DronChaseState(this, chaseRadius, runSpeed);
        DronKillState dronKillState = new DronKillState(this);
        DronDeathState dronDeathState = new DronDeathState(this);

        DronMoveTransition dronMoveTransition = new DronMoveTransition(this, DronState.Idle);
        DronInPlayerTransition dronChasePlayerTransition = new DronInPlayerTransition(this, DronState.Chase, chaseRadius);
        DronCatchPlayerTransition dronCatchPlayerTransition = new DronCatchPlayerTransition(this, DronState.Kill, killRadius);
        DronDieTransition dronDieTransition = new DronDieTransition(this, DronState.Dead);

        dronPatrolState.AddTransition(dronMoveTransition);
        dronPingState.AddTransition(dronMoveTransition);

        dronIdleState.AddTransition(dronChasePlayerTransition);
        dronPatrolState.AddTransition(dronChasePlayerTransition);
        dronPingState.AddTransition(dronChasePlayerTransition);

        dronChaseState.AddTransition(dronCatchPlayerTransition);

        dronIdleState.AddTransition(dronDieTransition);
        dronPatrolState.AddTransition(dronDieTransition);
        dronPingState.AddTransition(dronDieTransition);
        dronChaseState.AddTransition(dronDieTransition);
        dronKillState.AddTransition(dronDieTransition);

        AddState(dronIdleState, DronState.Idle);
        AddState(dronPatrolState, DronState.Patrol);
        AddState(dronPingState, DronState.Ping);
        AddState(dronChaseState, DronState.Chase);
        AddState(dronKillState, DronState.Kill);
        AddState(dronDeathState, DronState.Dead);
    }

    private void FixedUpdate()
    {
        if (!IsServer) return;
        if (targetPlayer == null) return;

        Vector3 targetPlayerPos = targetPlayer.transform.position;
        Vector3 playerVec = (targetPlayerPos - transform.position).normalized;
        lookVec = playerVec;
    }

    public void SetAnimation(string name, bool value)
    {
        anim.SetBool(name, value);
        SetAnimationClientRpc(name, value);
    }



    [ClientRpc]
    private void SetAnimationClientRpc(string name, bool value)
    {
        anim.SetBool(name, value);
    }

    private bool RayObstacle(Vector3 pos, Vector3 lookVec, float destance)
    {
        return Physics.Raycast(pos, lookVec, destance, obstacleMask);
    }

    public Collider RayPlayer(float radius)
    {
        Vector3 pos = headTrs.position;

        RaycastHit[] allPlayers = Physics.RaycastAll(pos, lookVec, radius, playerMask);
        if (allPlayers.Length == 0) return null;

        float minDistance = float.MaxValue;
        Collider targetPlayer = null;
        foreach (RaycastHit player in allPlayers)
        {
            if (Vector3.Distance(player.transform.position, pos) < minDistance)
            {
                targetPlayer = player.collider;
            }
        }

        Vector3 targetPos = targetPlayer.transform.position;
        Debug.DrawLine(pos, targetPos, Color.black);

        return targetPlayer;
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

        //if (IsObstacle && RayObstacle(pos, lookVec, minDistance))
        //    return null;

        Vector3 targetPos = targetPlayer.transform.position;
        Debug.DrawLine(pos, targetPos, Color.red);

        return targetPlayer;
    }

    public Collider ViewingPlayer(float radius)
    {
        List<Collider> players = new List<Collider>();

        Vector3 pos = headTrs.position;
        Vector3 eulerAngles = headTrs.eulerAngles;

        float lookingAngle = eulerAngles.y;  //ĳ���Ͱ� �ٶ󺸴� ������ ����
        Vector3 rightDir = AngleToDirX(lookingAngle + angle * 0.5f);
        Vector3 leftDir = AngleToDirX(lookingAngle - angle * 0.5f);
        Vector3 upDir = AngleToDirY(lookingAngle, true);
        Vector3 downDir = AngleToDirY(lookingAngle, false);
        Vector3 lookDir = AngleToDirX(lookingAngle);

#if UNITY_EDITOR
        Debug.DrawRay(pos, rightDir * radius, Color.blue);
        Debug.DrawRay(pos, leftDir * radius, Color.blue);
        Debug.DrawRay(pos, upDir * radius, Color.blue);
        Debug.DrawRay(pos, downDir * radius, Color.blue);
        Debug.DrawRay(pos, lookDir * radius, Color.cyan);
#endif

        Collider[] allPlayers = Physics.OverlapSphere(pos, radius, playerMask);
        if (allPlayers.Length == 0) return null;
        foreach (Collider player in allPlayers)
        {
            Vector3 targetPos = player.transform.position;
            Vector3 targetDir = (targetPos - pos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;

            if (targetAngle <= angle * 0.5f)
            {
                //player ������
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

    private Vector3 AngleToDirX(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

    private Vector3 AngleToDirY(float angle1, bool isUp)
    {
        float radian1 = angle1 * Mathf.Deg2Rad;
        float radian2 = (angle * 0.5f) * Mathf.Deg2Rad;

        Vector3 angleVec = isUp == true ? new Vector3(0f, Mathf.Sin(radian2), 0f) : new Vector3(0f, -Mathf.Sin(radian2), 0f);
        return new Vector3(Mathf.Sin(radian1), 0, Mathf.Cos(radian1)) + angleVec;
    }

    public void SetPingPos(Vector3 pos)
    {
        if (!IsServer) return;

        UnityEngine.AI.NavMeshHit hit;

        if (UnityEngine.AI.NavMesh.SamplePosition(pos, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
        {
            pingPos = hit.position;

            if (currentState == DronState.Idle || currentState == DronState.Patrol)
            {
                ChangeState(DronState.Ping);
            }
        }
    }

    public void KillPlayerAnimationEvent() //�ִϸ��̼��� ����ȭ �Ǵϱ� �ڵ��?�Ű� �Ƚᵵ �ɵ�?
    {
        if (!IsServer) return;

        var player = targetPlayer.GetComponent<PlayerController>();

        PlayerManager.Instance.PlayerDie(EnumList.DeadType.Monster, player.OwnerClientId);
        IsKill = true;
    }

    public void JumpScare() //��ǻ�?�ִϸ��̼��� �Ǿ� �־ �÷��̾ �� �����̰� ������ �ٶ󺸰� ������ �ϸ� �� ��?
    {
        var player = targetPlayer.GetComponent<PlayerController>();

        JumpScareClientRPC(player.OwnerClientId.GetRPCParams());
    }

    [ClientRpc]
    private void JumpScareClientRPC(ClientRpcParams param)
    {
        //PlayerController player = PlayerManager.Instance.FindPlayerControllerToID(playerId);
        //player == null �̰� �´µ�
        jsVcamTrs.Priority = 500;
        //player.Input.Disable();
        //player.enabled = false;
    }


    public void SetMonsterDeath()
    {
        if (!IsServer) return;

        IsDead = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void PingServerRPC(Vector3 pos)
    {

        SetPingPos(pos);

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
