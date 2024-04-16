using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using FSM_System.Netcode;
using System.Threading.Tasks;

public enum SculptureState
{
    Patrol,
    Chase,
    Kill
};

public class SculptureFSM : FSM_Controller_Netcode<SculptureState>
{
    public NavMeshAgent nav;
    public LayerMask playerMask;

    public SculptureState nowState;

    [HideInInspector] public Collider targetPlayer;
    [HideInInspector] public bool IsKill;
    [HideInInspector] public float currentCoolTime;

    [Header("Values")]
    [SerializeField] private float moveRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float killRadius;
    [SerializeField] private float moveInterval;
    [SerializeField] private float chaseInterval;
    [SerializeField] private float moveFrame;
    [SerializeField] private float chaseFrame;

    public LayerMask obstacleMask;

    readonly float frame = 0.1f;

    private void Start()
    {
        if (!IsServer)
        {
            nav.enabled = false;
            return;
        }

        base.Awake();

        nav.isStopped = true;

        InitializeStates();
        ChangeState(SculptureState.Patrol);
    }

    private void InitializeStates()
    {
        SculpturePatrolState sculpturePatrolState = new SculpturePatrolState(this, moveRadius, moveInterval, moveFrame);
        SculptureChaseState sculptureChaseState = new SculptureChaseState(this, chaseRadius, killRadius, chaseInterval, chaseFrame);
        SculptureKillState sculptureKillState = new SculptureKillState(this);

        SculptureFindPlayerTransition sculptureFindPlayerTransition = new SculptureFindPlayerTransition(this, SculptureState.Chase, chaseRadius);
        //SculptureFindPlayerTransition sculptureCatchPlayerTransition = new SculptureFindPlayerTransition(this, SculptureState.Kill, killRadius);

        sculpturePatrolState.AddTransition(sculptureFindPlayerTransition);
        //sculptureChaseState.AddTransition(sculptureCatchPlayerTransition);

        AddState(sculpturePatrolState, SculptureState.Patrol);
        AddState(sculptureChaseState, SculptureState.Chase);
        AddState(sculptureKillState, SculptureState.Kill);
    }

    public bool FrameMove(float timer, Vector3 pos)
    {
        currentCoolTime += Time.deltaTime;
        if (currentCoolTime >= timer)
        {
            currentCoolTime = 0f;
            nav.Warp(pos);
            return true;
        }
        return false;
    }

    public void LookAt(Vector3 pos)
    {
        Vector3 lookAtPosition = new Vector3(pos.x, transform.position.y, pos.z);

        transform.LookAt(lookAtPosition);
    }

    public Collider CirclePlayer(float radius)
    {
        Collider[] allPlayers = Physics.OverlapSphere(transform.position, radius, playerMask);
        if (allPlayers.Length == 0) return null;

        float minDistance = float.MaxValue;
        Collider targetPlayer = null;
        foreach (Collider player in allPlayers)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < minDistance)
            {
                targetPlayer = player;
            }
        }

        //if (IsObstacle && RayObstacle(pos, lookVec, minDistance))
        //    return null;

        Vector3 targetPos = targetPlayer.transform.position;
        Debug.DrawLine(transform.position, targetPos, Color.red);

        return targetPlayer;
    }

    public void KillPlayer()
    {
        if (!IsServer) return;

        var player = targetPlayer.GetComponent<PlayerController>();

        PlayerManager.Instance.PlayerDie(EnumList.DeadType.Monster, player.OwnerClientId);
        IsKill = true;
    }

    public List<Vector3> SamplePathPositions(NavMeshPath path, float interval)
    {
        List<Vector3> sampledPath = new List<Vector3>();
        foreach (Vector3 vertex in path.corners)
        {
            sampledPath.Add(vertex);
        }

        for (int i = 0; i < sampledPath.Count - 1; i++)
        {
            float segmentLength = Vector3.Distance(sampledPath[i], sampledPath[i + 1]);
            if (segmentLength > interval)
            {
                int numSegments = Mathf.CeilToInt(segmentLength / interval);
                Vector3 segmentDirection = (sampledPath[i + 1] - sampledPath[i]).normalized;
                float segmentInterval = segmentLength / numSegments;

                for (int j = 1; j < numSegments; j++)
                {
                    Vector3 newPoint = sampledPath[i] + segmentDirection * (segmentInterval * j);
                    sampledPath.Insert(i + j, newPoint);
                }
            }
        }

        return sampledPath;
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
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.DrawWireSphere(transform.position, killRadius);
    }
#endif
}
