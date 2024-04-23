using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaderState : MonsterStateRoot
{
    private PlayerController player;

    private float idleDuration = 3f;
    private float currentTime = 0;

    public RaderState(MonsterFSM controller) : base(controller) { }

    protected override void EnterState()
    {
        if (!IsServer) return;

        player = monsterFSM.targetPlayer.GetComponent<PlayerController>();
        monsterFSM.SetAnimation("Idle", true);
        //NetworkSoundManager.Play3DSound("MonsterHowling", monsterFSM.transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (currentTime >= idleDuration)
        {
            controller.ChangeState(MonsterState.Patrol);
        }
        else
        {
            currentTime += Time.deltaTime;
            if (player.Input.MoveVecter != Vector2.zero)
            {
                controller.ChangeState(MonsterState.Chase);
            }
        }
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Idle", false);
        currentTime = 0;
    }
}
