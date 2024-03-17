using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FSM_System.Netcode;

public class PingState : MonsterStateRoot
{
    private float speed;

    public PingState(MonsterFSM controller, float speed) : base(controller) 
    {
        this.speed = speed;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Work", true);
        NetworkSoundManager.Play3DSound("MonsterPing", monsterFSM.transform.position, 0.1f, 60f, SoundType.SFX, AudioRolloffMode.Linear);

        nav.speed = speed;

        Vector3 pos = monsterFSM.pingPos;
        nav.SetDestination(pos);
    }

    protected override void UpdateState()
    {
        base.UpdateState();
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        monsterFSM.SetAnimation("Work", false);

        nav.SetDestination(monsterFSM.transform.position);
    }
}
