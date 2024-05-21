using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class MonsterController : ObjectControllerBase
{



    public override void Move(Vector3 pos)
    {
    }

    public override void Rotate(Vector3 dir)
    {
    }

}
