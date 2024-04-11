using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DronPatrolState : DronStateRoot
{
    private Vector3 targetPos;
    private float range;
    private float speed;

    Vector3 point;

    public DronPatrolState(DronFSM controller, float radius, float speed) : base(controller)
    {
        range = radius;
        this.speed = speed;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        dronFSM.SetAnimation("Work", true);
        NetworkSoundManager.Play3DSound("MonsterHowling", dronFSM.transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);

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

        dronFSM.SetAnimation("Work", false);

        nav.SetDestination(dronFSM.transform.position);
    }

    private bool RandomPoint(float range, out Vector3 result) //약간 고치기
    {
        ulong randomPlayer = PlayerManager.Instance.alivePlayer[Random.Range(0, PlayerManager.Instance.alivePlayer.Count)].clientId;
        PlayerController pc = PlayerManager.Instance.FindPlayerControllerToID(randomPlayer);

        for (int i = 0; i < 400; i++)
        {
            Vector3 randomPoint = pc.transform.position + (Random.insideUnitSphere * range);
            NavMeshHit hit;

            //Vector3.Distance(monsterFSM.transform.position, hit.position) >= range / 2f
            //최소 이동거리 양이 정찰범위의 절반은 움직여야 한다.
            //Vector3.Distance(monsterFSM.targetPlayer.transform.position, hit.position) <= range / 4f
            //해당 위치 근처에 플레이어가 잇어야 한다(최대 이동범위의 1/5정도)

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        Debug.Log("어라?");
        return false;
    }
}
