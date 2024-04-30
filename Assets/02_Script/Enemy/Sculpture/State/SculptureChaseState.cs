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

        NetworkSoundManager.Play3DSound("SculptureRaderPlayer", sculptureFSM.transform.position, 0.1f, 45f, SoundType.SFX, AudioRolloffMode.Linear);
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (sculptureFSM.FrameMove(frame, GenerateVector()))
        {
            NetworkSoundManager.Play3DSound("SculptureMove", sculptureFSM.transform.position, 0.1f, 40f, SoundType.SFX, AudioRolloffMode.Linear);
            CatchPlayerRader();

            foreach (var item in sculptureFSM.CirclePlayers(chaseRadius))
            {
                Vector3 pos = sculptureFSM.transform.position;
                Vector3 dir = (item.transform.position - pos).normalized;
                float distance = Mathf.Abs(Vector3.Distance(pos, item.transform.position));

                PlayerDarkness playerDarkness = item.GetComponent<PlayerDarkness>();
                playerDarkness.Bliend(playerDarkness.OwnerClientId);

                //if (!sculptureFSM.RayObstacle(pos, dir, distance)) 벽 감지 시
            }

            sculptureFSM.LookAt(sculptureFSM.targetPlayer.transform.position);
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

            if (sculptureFSM.SamplePathPositions(navMeshPath).Count <= 1) //인덱스가 0이 최대다
                return playerPos;

            return sculptureFSM.SamplePathPositions(navMeshPath)[1];
            //0번째 위치는 무조건 제자리이다
        }
        else
        {
            sculptureFSM.ChangeState(SculptureState.Patrol);
            return Vector3.zero;
        }
    }//IdleState
}
