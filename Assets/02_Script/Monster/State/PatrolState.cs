using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;
using UnityEngine.AI;

public class PatrolState : MonsterStateRoot
{
    private Vector3 targetPos;
    private float range;
    private float speed;

    Vector3 point;

    public PatrolState(MonsterFSM controller, float radius, float speed) : base(controller) 
    {
        range = radius;
        this.speed = speed;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Work", true);
        NetworkSoundManager.Play3DSound("MonsterHowling", monsterFSM.transform.position, 0.1f, 30f);

        nav.speed = speed;

        if (RandomPoint(targetPos, range, out point))
        {
            targetPos = point;
            nav.SetDestination(targetPos);
        }
    }

    protected override void UpdateState()
    {

        if(!IsServer) return;

        NetworkSoundManager.Play3DSound("MonsterHowling", monsterFSM.transform.position, 0.1f, 30f);
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Work", false);

        nav.SetDestination(monsterFSM.transform.position);
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
