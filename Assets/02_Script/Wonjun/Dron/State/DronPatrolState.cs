using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class DronPatrolState : DronStateRoot
{
    private float range;
    private float speed;
    private ParticleSystem spark;

    public DronPatrolState(DronFSM controller, float radius, float speed, ParticleSystem sparks) : base(controller)
    {
        range = radius;
        this.speed = speed;
        this.spark = sparks;
    }

    protected override void EnterState()
    {
        spark.Play();

        if (!IsServer) return;

       
        Debug.Log("patrol����");
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

        // ����� �������� ���� ��ġ�� �����Ͽ� ����
        nav.SetDestination(dronFSM.transform.position);
    }

}
