using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronChaseState : DronStateRoot
{
    private float radius;
    private float speed;

    public DronChaseState(DronFSM controller, float radius, float speed) : base(controller)
    {
        this.radius = radius;
        this.speed = speed;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        dronFSM.SetAnimation("Run", true);
        NetworkSoundManager.Play3DSound("FindPlayer", transform.position, 0.1f, 40f, SoundType.SFX, AudioRolloffMode.Linear);

        nav.speed = speed;
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        //Vector3 playerPos = monsterFSM.targetPlayer.transform.position;
        //nav.SetDestination(playerPos);

        Collider player = dronFSM.CirclePlayer(radius);

        if (player != null)
        {
            dronFSM.targetPlayer = player;

            Vector3 playerPos = dronFSM.targetPlayer.transform.position;
            nav.SetDestination(playerPos);
        }
        else
        {
            dronFSM.ChangeState(DronState.Idle);
        }
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        dronFSM.SetAnimation("Run", false);

        nav.SetDestination(dronFSM.transform.position);
    }
}
