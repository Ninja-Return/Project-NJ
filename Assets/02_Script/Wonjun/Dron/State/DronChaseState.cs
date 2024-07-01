using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronChaseState : DronStateRoot
{
    private float radius;
    private float speed;
    private Light dronLight;

    public DronChaseState(DronFSM controller, float radius, float speed, Light dronLight) : base(controller)
    {
        this.radius = radius;
        this.speed = speed;
        this.dronLight = dronLight;
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
            dronFSM.DronStun(1.5f);  // n�� ���� ���� (�ð��� ���ϴ� ��� ����)

            Collider[] enemys = Physics.OverlapSphere(dronFSM.transform.position, 40f);
            foreach (Collider enemy in enemys)
            {
                if (enemy.TryGetComponent(out IEnemyInterface enemyInterface))
                {
                    enemyInterface.Ping(dronFSM.targetPlayer.transform.position);
                }
            }
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

            //���� �����ߴٴ� ����Ʈ�� �־���(�������Ҹ�&�����̳� �Һ� �����ɷ�)
            dronLight.color = Color.red;
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

        dronLight.color = Color.green;
    }
}
