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
        NetworkSoundManager.Play3DSound("MonsterHowling", monsterFSM.transform.position, 0.1f, 60f, SoundType.SFX, AudioRolloffMode.Linear);
        
        nav.speed = speed;

        if (RandomPoint(range, out point))
        {
            nav.SetDestination(point);
        }
    }

    protected override void UpdateState()
    {

    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Work", false);

        nav.SetDestination(monsterFSM.transform.position);
    }

    private bool RandomPoint(float range, out Vector3 result) //약간 고치기
    {
        for (int i = 0; i < 200; i++)
        {
            Vector3 randomPoint = monsterFSM.transform.position + Random.insideUnitSphere * range;
            NavMeshHit hit;

            //Vector3.Distance(monsterFSM.transform.position, hit.position) >= range / 2f <= 최소 이동거리 양

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)
                && Physics.OverlapSphere(hit.position, range / 5f, playerMask).Length > 0)
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
