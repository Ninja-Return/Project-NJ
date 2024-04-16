using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SculpturePatrolState : SculptureStateRoot
{
    private float radius;
    private float frame;

    private Vector3 point;
    private List<Vector3> points = new List<Vector3>();

    public SculpturePatrolState(SculptureFSM controller, float radius, float frame) : base(controller)
    {
        this.radius = radius;
        this.frame = frame;
    }

    protected override void EnterState()
    {
        //if (!IsServer) return;

        //GeneratePath();
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (points.Count == 0)
        {
            GeneratePath();
        }

        if (sculptureFSM.FrameMove(frame, points[0]))
        {
            points.Remove(points[0]);
        }
        else
        {
            sculptureFSM.LookAt(points[0]);
        }
    }

    protected override void ExitState() 
    {
        points.Clear();
    }

    private void GeneratePath()
    {
        NavMeshPath navMeshPath = new NavMeshPath();

        if (RandomPoint(radius, out point))
        {
            NetworkSoundManager.Play3DSound("SculptureMove", sculptureFSM.transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);

            nav.CalculatePath(point, navMeshPath);
            points = sculptureFSM.SamplePathPositions(navMeshPath);
        }
    }

    private bool RandomPoint(float range, out Vector3 result) //�ణ ��ġ��
    {
        ulong randomPlayer = PlayerManager.Instance.alivePlayer[Random.Range(0, PlayerManager.Instance.alivePlayer.Count)].clientId;
        PlayerController pc = PlayerManager.Instance.FindPlayerControllerToID(randomPlayer);

        for (int i = 0; i < 400; i++)
        {
            Vector3 randomPoint = pc.transform.position + (Random.insideUnitSphere * range);
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
