using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SculptureChaseState : SculptureStateRoot
{
    private float chaseRadius;
    private float killRadius;
    private float frame;

    public SculptureChaseState(SculptureFSM controller, float chaseRadius, float killRadius, float frame) : base(controller)
    {
        this.chaseRadius = chaseRadius;
        this.killRadius = killRadius;
        this.frame = frame;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        NetworkSoundManager.Play3DSound("SculptureFindPlayer", sculptureFSM.transform.position, 0.1f, 45f, SoundType.SFX, AudioRolloffMode.Linear);
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (sculptureFSM.FrameMove(frame, GenerateVector()))
        {
            NetworkSoundManager.Play3DSound("SculptureChase", sculptureFSM.transform.position, 0.1f, 40f, SoundType.SFX, AudioRolloffMode.Linear);

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
    } //KillState

    private Vector3 GenerateVector()
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        Collider player = sculptureFSM.CirclePlayer(chaseRadius);

        if (player != null)
        {
            sculptureFSM.targetPlayer = player;

            Vector3 playerPos = player.transform.position;

            nav.CalculatePath(playerPos, navMeshPath);

            if (sculptureFSM.SamplePathPositions(navMeshPath).Count <= 1)
                return playerPos;

            return sculptureFSM.SamplePathPositions(navMeshPath)[1];
        }
        else
        {
            sculptureFSM.ChangeState(SculptureState.Patrol);
            return Vector3.zero;
        }
    }//IdleState
}
