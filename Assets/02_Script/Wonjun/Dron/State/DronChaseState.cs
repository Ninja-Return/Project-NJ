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
        Debug.Log("chase들어옴");
        nav.speed = speed;
        dronLight.color = Color.red;
        warningSign.SetActive(true);
        // 플레이어를 스턴시키기 위한 코드 추가
        if (dronFSM.targetPlayer != null)
        {
            dronFSM.PlayerStun(1.5f);  // n초 동안 스턴 (시간은 원하는 대로 조절)

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
            warningSign.transform.Rotate(new Vector3(0f, 20f, 0f) * Time.deltaTime * 10f);
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
        dronLight.color = new Color(255,151,0,255);
        nav.SetDestination(dronFSM.transform.position);

    }
}
