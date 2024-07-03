using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronChaseState : DronStateRoot
{
    private float radius;
    private float speed;
    private Light dronLight;
    private GameObject warningSign;

    public DronChaseState(DronFSM controller, float radius, float speed, Light dronLight, GameObject warningSign) : base(controller)
    {
        this.radius = radius;
        this.speed = speed;
        this.dronLight = dronLight;
        this.warningSign = warningSign;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;
        NetworkSoundManager.Play3DSound("DronBite", transform.position, 0.1f, 40f, SoundType.SFX, AudioRolloffMode.Linear);
        Debug.Log("chase����");
        nav.speed = speed;
        dronLight.color = Color.red;
        warningSign.SetActive(true);
        // �÷��̾ ���Ͻ�Ű�� ���� �ڵ� �߰�
        if (dronFSM.targetPlayer != null)
        {
            dronFSM.PlayerStun(1.5f);  // n�� ���� ���� (�ð��� ���ϴ� ��� ����)

            Collider[] enemys = Physics.OverlapSphere(dronFSM.transform.position, 40f);
            foreach (Collider enemy in enemys)
            {
                if (enemy.TryGetComponent(out IEnemyInterface enemyInterface))
                {
                    enemyInterface.Ping(dronFSM.targetPlayer.transform.position);
                }
            }

            //NetworkSoundManager.Play3DSound("DronAlert", dronFSM.transform.position, 0.1f, 40f);
            //DronAlert
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
        warningSign.SetActive(false);
        nav.SetDestination(dronFSM.transform.position);

    }
}
