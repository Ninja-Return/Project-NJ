using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SculpturePingState : SculptureStateRoot
{
    private float frame;
    private List<Vector3> points = new List<Vector3>();

    public SculpturePingState(SculptureFSM controller, float frame) : base(controller)
    {
        this.frame = frame;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        GeneratePath();
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (points.Count == 0)
        {
            sculptureFSM.ChangeState(SculptureState.Patrol);
            return;
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
        if (!IsServer) return;

        points.Clear();
        sculptureFSM.IsPing = false;
    }

    private void GeneratePath()
    {
        NetworkSoundManager.Play3DSound("SculptureMove", sculptureFSM.transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);

        NavMeshPath navMeshPath = new NavMeshPath();
        nav.CalculatePath(sculptureFSM.pingPos, navMeshPath);
        points = sculptureFSM.SamplePathPositions(navMeshPath);
    }
}
