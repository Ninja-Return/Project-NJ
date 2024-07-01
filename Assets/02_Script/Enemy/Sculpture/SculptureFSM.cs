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
    Ping,
    Chase,
    Kill,
    Dead
};

public class SculptureFSM : FSM_Controller_Netcode<SculptureState>, IEnemyInterface
{
    public NavMeshAgent nav;
    public Transform headTrs;
    public LayerMask playerMask;

    public SculptureState nowState => currentState;

    [HideInInspector] public Collider targetPlayer { get; set; }
    [HideInInspector] public Vector3 pingPos { get; set; }
    [HideInInspector] public bool IsPing { get; set; }
    [HideInInspector] public bool IsDead { get; private set; }
    [HideInInspector] public bool IsKill { get; set; }
    [HideInInspector] public float currentCoolTime { get; set; }

    [Header("Prefab")]
    [SerializeField] private NetworkObject deadBody;

    [Header("Values")]
    [SerializeField] private float moveRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float killRadius;
    [SerializeField] private float minInterval;
    [SerializeField] private float maxInterval;
    [SerializeField] private float moveFrame;
    [SerializeField] private float chaseFrame;

    public LayerMask obstacleMask;

    readonly Vector3 deadbodyPivot = new Vector3(0, 0.1f, 0);
    readonly float frame = 0.2f;

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
        SculpturePatrolState sculpturePatrolState = new SculpturePatrolState(this, moveRadius, moveFrame);
        SculpturePingState sculpturePingState = new SculpturePingState(this, moveFrame);
        SculptureChaseState sculptureChaseState = new SculptureChaseState(this, chaseRadius, killRadius, chaseFrame);
        SculptureKillState sculptureKillState = new SculptureKillState(this);
        SculptureDeathState sculptureDeathState = new SculptureDeathState(this);

        SculpturePingTransition sculpturePingTransition = new SculpturePingTransition(this, SculptureState.Ping);
        SculptureFindPlayerTransition sculptureFindPlayerTransition = new SculptureFindPlayerTransition(this, SculptureState.Chase, chaseRadius);
        SculptureDieTransition sculptureDieTransition = new SculptureDieTransition(this, SculptureState.Dead);
        //SculptureFindPlayerTransition sculptureCatchPlayerTransition = new SculptureFindPlayerTransition(this, SculptureState.Kill, killRadius);

        sculpturePatrolState.AddTransition(sculpturePingTransition);

        sculpturePatrolState.AddTransition(sculptureFindPlayerTransition);
        //sculptureChaseState.AddTransition(sculptureCatchPlayerTransition);

        sculpturePatrolState.AddTransition(sculptureDieTransition);
        sculptureChaseState.AddTransition(sculptureDieTransition);
        sculptureKillState.AddTransition(sculptureDieTransition);

        AddState(sculpturePatrolState, SculptureState.Patrol);
        AddState(sculpturePingState, SculptureState.Ping);
        AddState(sculptureChaseState, SculptureState.Chase);
        AddState(sculptureKillState, SculptureState.Kill);
        AddState(sculptureDeathState, SculptureState.Dead);
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

    public bool RayObstacle(Vector3 pos, Vector3 lookVec, float destance)
    {
        return Physics.Raycast(pos, lookVec, destance, obstacleMask);
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

    public Collider[] CirclePlayers(float radius)
    {

        Collider[] allPlayers = Physics.OverlapSphere(transform.position, radius, playerMask);

        return allPlayers;

    }

    public void KillPlayer()
    {
        if (!IsServer) return;

        StartCoroutine(KillPlayerCor());
    }

    public List<Vector3> SamplePathPositions(NavMeshPath path)
    {
        List<Vector3> sampledPath = new List<Vector3>();
        foreach (Vector3 vertex in path.corners)
        {
            sampledPath.Add(vertex);
        }

        for (int i = 0; i < sampledPath.Count - 1; i++)
        {
            float segmentLength = Vector3.Distance(sampledPath[i], sampledPath[i + 1]);

            if (segmentLength > maxInterval)
            {
                int numSegments = Mathf.CeilToInt(segmentLength / maxInterval);
                Vector3 segmentDirection = (sampledPath[i + 1] - sampledPath[i]).normalized;
                float segmentInterval = segmentLength / numSegments;

                for (int j = 1; j < numSegments; j++)
                {
                    Vector3 newPoint = sampledPath[i] + segmentDirection * (segmentInterval * j);
                    sampledPath.Insert(i + j, newPoint);
                }
            }
            else if (segmentLength < minInterval)
            {
                Vector3 orgPoint = sampledPath[i];
                while (segmentLength < minInterval && !(i + 1 >= sampledPath.Count))
                {
                    Vector3 nextPoint = sampledPath[i + 1];
                    segmentLength = Vector3.Distance(orgPoint, sampledPath[i + 1]);

                    if (segmentLength > maxInterval)
                        break;

                    sampledPath.RemoveAt(i + 1);
                    sampledPath[i] = nextPoint;
                }
            }
        }
        
        return sampledPath;
    }

    public void Ping(Vector3 pos)
    {
        if (IsDead) return;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas))
        {
            pingPos = hit.position;
            IsPing = true;
        }
    }

    public void Death()
    {
        DeathServerRpc();
    }

    public void CreateDeadbody()
    {
        NetworkObject sculptureDeadbody = Instantiate(deadBody, transform.position + deadbodyPivot, transform.rotation);
        sculptureDeadbody.Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeathServerRpc()
    {
        IsDead = true;
    }

    private IEnumerator KillPlayerCor()
    {
        var player = targetPlayer.GetComponent<PlayerDarkness>();
        player.Bliend(player.OwnerClientId, true);

        yield return new WaitForSeconds(0.5f);

        PlayerManager.Instance.PlayerDie(EnumList.DeadType.Sculpture, player.OwnerClientId);
        IsKill = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.DrawWireSphere(transform.position, killRadius);

        if (targetPlayer != null)
        {
            Vector3 dir = targetPlayer.transform.position - transform.position;
            Gizmos.DrawRay(new Ray(transform.position, dir));
        }
    }
#endif
}
