using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public class DeathState : MonsterStateRoot
{
    public DeathState(MonsterFSM controller) : base(controller)
    {

    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        monsterFSM.nav.isStopped = true;
        monsterAnim.SetAnimation("IsDeath");
        NetworkSoundManager.Play3DSound("MonsterDie", monsterFSM.transform.position, 0.1f, 45f, SoundType.SFX, AudioRolloffMode.Linear);
    }

    protected override void UpdateState()
    {
        
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        monsterFSM.nav.isStopped = false;
        monsterAnim.SetAnimation("Death", false); //Rader
    }
}
