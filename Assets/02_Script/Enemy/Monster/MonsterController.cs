using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class MonsterController : ObjectControllerBase
{

    public NavMeshAgent MonsterAgnet { get; private set; }
    public Rigidbody MonsterRigid { get; private set; }
    public bool IsOnNavMesh => MonsterAgnet.isOnNavMesh;
    public bool IsStopped => MonsterAgnet.isStopped;
    public bool HasPath => MonsterAgnet.hasPath;
    public bool IsMoving => MonsterAgnet.remainingDistance > 0.1f;

    protected virtual void Awake()
    {
        
        MonsterAgnet = GetComponent<NavMeshAgent>();
        MonsterRigid = GetComponent<Rigidbody>();

    }

    public override void Move(Vector3 pos)
    {

        if (MonsterAgnet.isOnNavMesh)
        {

            MonsterAgnet.SetDestination(pos);

        }

    }

    public override void Rotate(Vector3 dir)
    {

        if(MonsterAgnet.angularSpeed == 0)
        {

            transform.forward = dir;

        }

    }

    public override void Stop()
    {

        MonsterAgnet.isStopped = true;

    }

    public override void Continue()
    {

        MonsterAgnet.isStopped = false;

    }

    public Collider ViewingAndGetClosest(float radius, float angle, LayerMask targetMask, LayerMask obstacleMast)
    {
        List<Collider> casted = new List<Collider>();

        Vector3 pos = transform.position;
        Vector3 eulerAngles = transform.eulerAngles;

        float lookingAngle = eulerAngles.y;  //캐릭터가 바라보는 방향의 각도
        Vector3 lookDir = AngleToDirX(lookingAngle);

        Collider[] allPlayers = Physics.OverlapSphere(pos, radius, targetMask);

        foreach (Collider player in allPlayers)
        {
            Vector3 targetPos = player.transform.position;
            Vector3 targetDir = (targetPos - pos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
            float playerDistance = Vector3.Distance(player.transform.position, pos);

            if (targetAngle <= angle * 0.5f && !RayObstacle(pos, targetDir, playerDistance, obstacleMast))
            {

                casted.Add(player);

            }
        }

        float minDistance = float.MaxValue;
        Collider target = null;
        foreach (Collider cast in casted)
        {
            float playerDistance = Vector3.Distance(cast.transform.position, pos);
            if (playerDistance < minDistance)
            {
                target = cast;
            }
        }

        return target;
    }

    public Collider GetClosest(float radius, LayerMask targetMask)
    {
        Vector3 pos = transform.position;

        Collider[] allPlayers = Physics.OverlapSphere(pos, radius, targetMask);
        float minDistance = float.MaxValue;
        Collider target = null;
        foreach (Collider cast in allPlayers)
        {
            float playerDistance = Vector3.Distance(cast.transform.position, pos);
            if (playerDistance < minDistance)
            {
                target = cast;
            }
        }

        return target;
    }

    private bool RayObstacle(Vector3 pos, Vector3 lookVec, float destance, LayerMask obstacleMask)
    {
        return Physics.Raycast(pos, lookVec, destance, obstacleMask);
    }
    private Vector3 AngleToDirX(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

}
