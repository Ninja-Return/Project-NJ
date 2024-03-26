using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public class ChaseState : MonsterStateRoot
{
    private float radius;
    private float speed;

    public ChaseState(MonsterFSM controller, float radius, float speed) : base(controller)
    {
        this.radius = radius;
        this.speed = speed;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Run", true);
        NetworkSoundManager.Play3DSound("FindPlayer", transform.position, 0.1f, 90f, SoundType.SFX, AudioRolloffMode.Linear);

        nav.speed = speed;
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        //Vector3 playerPos = monsterFSM.targetPlayer.transform.position;
        //nav.SetDestination(playerPos);

        Collider player = monsterFSM.CirclePlayer(radius);

        if (player != null)
        {
            monsterFSM.targetPlayer = player;

            Vector3 playerPos = monsterFSM.targetPlayer.transform.position;
            nav.SetDestination(playerPos);
        }
        else
        {
            monsterFSM.ChangeState(MonsterState.Idle);
        }
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Run", false);

        nav.SetDestination(monsterFSM.transform.position);
    }
}
