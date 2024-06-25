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
        NetworkSoundManager.Play3DSound("DronBite", transform.position, 0.1f, 40f, SoundType.SFX, AudioRolloffMode.Linear);
        Debug.Log("chase����");
        nav.speed = speed;

        // �÷��̾ ���Ͻ�Ű�� ���� �ڵ� �߰�
        if (dronFSM.targetPlayer != null)
        {
            dronFSM.Stun(2.5f); // 5�� ���� ���� (�ð��� ���ϴ� ��� ����)
        }
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

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
