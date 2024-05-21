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

}
