using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using FSM_System.Netcode;
using DG.Tweening;
using Cinemachine;

public enum MonsterState
{
    Idle,
    Patrol,
    Ping,
    Chase,
    Kill,
    Dead
}

public class MonsterFSM : FSM_Controller_Netcode<MonsterState>, IEnemyInterface
{
    public MonsterAnimation monsterAnim;
    public NavMeshAgent nav;
    public Transform headTrs;
    public CinemachineVirtualCamera jsVcamTrs;
    public float angle;
    public Vector3 lookVec;
    public LayerMask playerMask;

    public MonsterState nowState;


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
        ChangeState(MonsterState.Idle);
    }

    private void InitializeStates()
    {
        IdleState idleState = new IdleState(this);
        PatrolState patrolState = new PatrolState(this, moveRadius, workSpeed);
        PingState pingState = new PingState(this, workSpeed);
        ChaseState chaseState = new ChaseState(this, chaseRadius, runSpeed);
        KillState killState = new KillState(this);
        DeathState deathState = new DeathState(this);

        MoveTransition moveTransition = new MoveTransition(this, MonsterState.Idle);
        InPlayerTransition chasePlayerTransition = new InPlayerTransition(this, MonsterState.Chase, chaseRadius);
        CatchPlayerTransition catchPlayerTransition = new CatchPlayerTransition(this, MonsterState.Kill, killRadius);
        DieTransition dieTransition = new DieTransition(this, MonsterState.Dead);

        patrolState.AddTransition(moveTransition);
        pingState.AddTransition(moveTransition);

        idleState.AddTransition(chasePlayerTransition);
        patrolState.AddTransition(chasePlayerTransition);
        pingState.AddTransition(chasePlayerTransition);

        chaseState.AddTransition(catchPlayerTransition);

        idleState.AddTransition(dieTransition);
        patrolState.AddTransition(dieTransition);
        pingState.AddTransition(dieTransition);
        chaseState.AddTransition(dieTransition);
        killState.AddTransition(dieTransition);

        AddState(idleState, MonsterState.Idle);
        AddState(patrolState, MonsterState.Patrol);
        AddState(pingState, MonsterState.Ping);
        AddState(chaseState, MonsterState.Chase);
        AddState(killState, MonsterState.Kill);
        AddState(deathState, MonsterState.Dead);
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

        float lookingAngle = eulerAngles.y;  //캐릭터가 바라보는 방향의 각도
        Vector3 lookDir = AngleToDirX(lookingAngle);
#if UNITY_EDITOR
        Vector3 rightDir = AngleToDirX(lookingAngle + angle * 0.5f);
        Vector3 leftDir = AngleToDirX(lookingAngle - angle * 0.5f);
        Vector3 upDir = AngleToDirY(lookingAngle, true);
        Vector3 downDir = AngleToDirY(lookingAngle, false);

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
            float playerDistance = Vector3.Distance(player.transform.position, pos);

            if (targetAngle <= angle * 0.5f && !Physics.Raycast(pos, targetDir, playerDistance, obstacleMask))
            {
                //player 감지됨
                players.Add(player);
                Debug.DrawLine(pos, targetPos, Color.red);
            }
        }

        if (players.Count == 0) return null;

        //가장 가까운 플레이어 감지
        float minDistance = float.MaxValue;
        Collider targetPlayer = null;
        foreach (Collider player in players)
        {
            float playerDistance = Vector3.Distance(player.transform.position, pos);
            if (playerDistance < minDistance)
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

        NavMeshHit hit;

        if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas))
        {
            pingPos = hit.position;

            if (currentState == MonsterState.Idle || currentState == MonsterState.Patrol)
            {
                ChangeState(MonsterState.Ping);
            }
        }
    }

    public void KillPlayerAnimationEvent() //애니메이션은 동기화 되니까 코드는 신경 안써도 될듯?
    {
        if (!IsServer) return;

        var player = targetPlayer.GetComponent<PlayerController>();

        PlayerManager.Instance.PlayerDie(EnumList.DeadType.Monster, player.OwnerClientId);
        IsKill = true;
    }

    public void JumpScare() //사실상 애니메이션이 되어 있어서 플레이어가 못 움직이고 괴물을 바라보게 고정만 하면 될 듯?
    {
        var player = targetPlayer.GetComponent<PlayerController>();

        JumpScareClientRPC(player.OwnerClientId.GetRPCParams());
    }

    [ClientRpc]
    private void JumpScareClientRPC(ClientRpcParams param)
    {
        //PlayerController player = PlayerManager.Instance.FindPlayerControllerToID(playerId);
        //player == null 이게 맞는듯
        jsVcamTrs.Priority = 500;
        //player.Input.Disable();
        //player.enabled = false;
    }

    public void Death()
    {
        DeathServerRpc();
    }

    [ServerRpc]
    public void DeathServerRpc()
    {
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
