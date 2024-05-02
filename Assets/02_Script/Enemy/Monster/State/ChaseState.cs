using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public class ChaseState : MonsterStateRoot
{
    private float radius;
    private float speed;

    private float currentTime = 0;
    private int slowdownCount = 3;
    readonly private float slowdownTime = 6f;
    readonly private float slowdownRatio = 0.9f;

    public ChaseState(MonsterFSM controller, float radius, float speed) : base(controller)
    {
        this.radius = radius;
        this.speed = speed;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        monsterAnim.SetAnimation("Run", true);
        NetworkSoundManager.Play3DSound("FindPlayer", transform.position, 0.1f, 40f, SoundType.SFX, AudioRolloffMode.Linear);

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
            monsterFSM.targetPlayer = player.GetComponent<PlayerController>();

            Vector3 playerPos = player.transform.position;
            nav.SetDestination(playerPos);
        }
        else
        {
            monsterFSM.ChangeState(MonsterState.Idle);
        }

        SpeedSlowdown();
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        monsterAnim.SetAnimation("Run", false);
        currentTime = 0f;

        nav.SetDestination(monsterFSM.transform.position);
    }

    private void SpeedSlowdown()
    {
        currentTime += Time.deltaTime;
        if (currentTime > slowdownTime && slowdownCount > 0)
        {
            currentTime = 0f;
            slowdownCount--;

            speed *= slowdownRatio;
            nav.speed = speed;
        }
    }
}
