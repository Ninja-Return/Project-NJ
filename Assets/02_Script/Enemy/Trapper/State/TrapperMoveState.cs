using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrapperMoveState : TrapperStateRoot
{
    private float idleDuration;
    private float currentTime;

    private float minTrapTime, maxTrapTime;
    private float radius;
    private Vector3 point;

    public TrapperMoveState(TrapperFSM controller, float radius, float min, float max) : base(controller)
    {
        this.radius = radius;
        minTrapTime = min;
        maxTrapTime = max;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        RoadPath();
        idleDuration = Random.Range(minTrapTime, maxTrapTime);
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (Vector3.Distance(nav.destination, controller.transform.position) <= 2f)
        {
            RoadPath();
        }

        if (currentTime >= idleDuration)
        {
            currentTime = 0f;
            idleDuration = Random.Range(minTrapTime, maxTrapTime);

            trapperFSM.SpawnTrap();
        }
        else
            currentTime += Time.deltaTime;
    }

    protected override void ExitState()
    {
        
    }

    private void RoadPath()
    {
        if (RandomPoint(radius, out point))
        {
            nav.SetDestination(point);
        }
    }

    private bool RandomPoint(float range, out Vector3 result) //약간 고치기
    {
        ulong randomPlayer = PlayerManager.Instance.alivePlayer[Random.Range(0, PlayerManager.Instance.alivePlayer.Count)].clientId;
        PlayerController pc = PlayerManager.Instance.FindPlayerControllerToID(randomPlayer);

        for (int i = 0; i < range * 15; i++) //감지범위만큼 검사횟수 늘리기
        {
            Vector3 randomPoint = pc.transform.position + (Random.insideUnitSphere * range);
            NavMeshHit hit;

            //Vector3.Distance(monsterFSM.transform.position, hit.position) >= range / 2f
            //최소 이동거리 양이 정찰범위의 절반은 움직여야 한다.
            //Vector3.Distance(monsterFSM.targetPlayer.transform.position, hit.position) <= range / 4f
            //해당 위치 근처에 플레이어가 잇어야 한다(최대 이동범위의 1/5정도)

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas) &&
                Vector3.Distance(trapperFSM.transform.position, hit.position) >= range / 2f)
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
