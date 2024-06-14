using System;
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
        NetworkSoundManager.Play3DSound("DronKill", transform.position, 0.1f, 40f, SoundType.SFX, AudioRolloffMode.Linear);
        Debug.Log("chase����");
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
            dronFSM.targetPlayer = player.GetComponent<PlayerController>();
            Vector3 playerPos = player.transform.position;
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

        nav.SetDestination(dronFSM.transform.position);
    }
}
