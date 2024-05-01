using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaderState : MonsterStateRoot
{
    private PlayerController player;

    private float idleDuration = 3f;
    private float currentTime = 0;

    readonly float raderTime = 0.8f;

    public RaderState(MonsterFSM controller) : base(controller) { }

    protected override void EnterState()
    {
        if (!IsServer) return;

        player = monsterFSM.targetPlayer;
        monsterAnim.SetAnimation("Idle", true); //Rader
        NetworkSoundManager.Play3DSound("RaderPlayer", monsterFSM.transform.position, 0.1f, 45f, SoundType.SFX, AudioRolloffMode.Linear);
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

            if (monsterFSM.IsPlayerMoving(player) && currentTime > raderTime)
            {
                controller.ChangeState(MonsterState.Chase);
            }
        }
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        monsterAnim.SetAnimation("Idle", false); //Rader
        currentTime = 0;
    }
}
