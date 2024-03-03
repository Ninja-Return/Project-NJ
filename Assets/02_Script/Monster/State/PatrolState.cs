using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;
using UnityEngine.AI;

public class PatrolState : MonsterStateRoot
{
    private Transform targetPos;
    private float range = 10f;

    Vector3 point;

    public PatrolState(MonsterFSM controller) : base(controller) { }

    protected override void EnterState()
    {
        if (!IsServer) return;

        if (RandomPoint(targetPos.position, range, out point))
        {
            targetPos.position = point;
            nav.SetDestination(targetPos.position);
        }
    }

    protected override void UpdateState()
    {
        
    }

    protected override void ExitState()
    {
        base.ExitState();
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
