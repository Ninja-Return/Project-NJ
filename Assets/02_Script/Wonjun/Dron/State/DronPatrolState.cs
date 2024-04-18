using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DronPatrolState : DronStateRoot
{
    private float range;
    private float speed;

    public DronPatrolState(DronFSM controller, float radius, float speed) : base(controller)
    {
        range = radius;
        this.speed = speed;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        NetworkSoundManager.Play3DSound("DronHowling", dronFSM.transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);

        nav.speed = speed;

        Vector3 randomPoint = Random.insideUnitSphere * range;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
        {
            nav.SetDestination(hit.position);
        }
    }

    protected override void UpdateState()
    {

    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        // 드론의 목적지를 현재 위치로 설정하여 멈춤
        nav.SetDestination(dronFSM.transform.position);
    }

}
