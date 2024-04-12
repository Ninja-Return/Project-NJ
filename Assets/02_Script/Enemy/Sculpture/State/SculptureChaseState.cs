using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SculptureChaseState : SculptureStateRoot
{
    private float chaseRadius;
    private float killRadius;
    private float interval;
    private float frame;

    public SculptureChaseState(SculptureFSM controller, float chaseRadius, float killRadius, float interval, float frame) : base(controller)
    {
        this.chaseRadius = chaseRadius;
        this.killRadius = killRadius;
        this.interval = interval;
        this.frame = frame;
    }

    protected override void EnterState() { }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (sculptureFSM.FrameMove(frame, GenerateVector()))
        {
            CatchPlayerRader();

            PlayerDarkness player = sculptureFSM.targetPlayer.GetComponent<PlayerDarkness>();
            player.Bliend(player.OwnerClientId);

            sculptureFSM.LookAt(player.transform.position);
        }
    }

    protected override void ExitState() { }

    private void CatchPlayerRader()
    {
        Collider targetPlayer = sculptureFSM.CirclePlayer(killRadius);
        if (targetPlayer != null)
        {
            sculptureFSM.ChangeState(SculptureState.Kill);
        }
    }

    private Vector3 GenerateVector()
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        Collider player = sculptureFSM.CirclePlayer(chaseRadius);

        if (player != null)
        {
            sculptureFSM.targetPlayer = player;

            Vector3 playerPos = player.transform.position;

            nav.CalculatePath(playerPos, navMeshPath);

            if (sculptureFSM.SamplePathPositions(navMeshPath, interval).Count <= 1)
                return playerPos;

            return sculptureFSM.SamplePathPositions(navMeshPath, interval)[1];
        }
        else
        {
            sculptureFSM.ChangeState(SculptureState.Patrol);
            return Vector3.zero;
        }
    }
}
