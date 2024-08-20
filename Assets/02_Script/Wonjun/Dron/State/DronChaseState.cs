using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DronChaseState : DronStateRoot
{
    private float radius;
    private float speed;
    private float raderTime;
    private float chaseTime;
    private Light dronLight;
    private TMP_Text timerText;

    private float currentTime;

    public DronChaseState(DronFSM controller, float radius, float speed, float raderTime, float chaseTime, Light dronLight, TMP_Text timerText) : base(controller)
    {
        this.radius = radius;
        this.speed = speed;
        this.raderTime = raderTime;
        this.chaseTime = chaseTime;
        this.dronLight = dronLight;
        this.timerText = timerText;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        currentTime = 0f;
        nav.speed = speed;
        //dronLight.color = Color.red;

        //if (dronFSM.targetPlayer != null)
        //{
        //    Collider[] enemys = Physics.OverlapSphere(dronFSM.transform.position, 40f);
        //    foreach (Collider enemy in enemys)
        //    {
        //        if (enemy.TryGetComponent(out IEnemyInterface enemyInterface))
        //        {
        //            enemyInterface.Ping(dronFSM.targetPlayer.transform.position);
        //        }
        //    }
        //}
        
        NetworkSoundManager.Play3DSound("DronKill", transform.position, 0.1f, 40f, SoundType.SFX, AudioRolloffMode.Linear);
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        timerText.transform.LookAt(dronFSM.targetPlayer.transform);
        currentTime += Time.deltaTime;

        if (currentTime < raderTime)
        {
            float progress = currentTime / raderTime;
            timerText.text = (progress * chaseTime).ToString("F1");
            return;
        }

        float time = currentTime - raderTime;
        if (time >= chaseTime)
        {
            dronFSM.ChangeState(DronState.Idle);
        }
        else
        {
            Vector3 playerPos = dronFSM.targetPlayer.transform.position;
            nav.SetDestination(playerPos);

            timerText.text = (chaseTime - time).ToString("F1");
        }



        //#범위 감지 기반#
        //Collider player = dronFSM.CirclePlayer(radius);
        //if (player != null)
        //{
        //    dronFSM.targetPlayer = player.GetComponent<PlayerController>();
        //    Vector3 playerPos = player.transform.position;
        //    nav.SetDestination(playerPos);
        //}
        //else
        //{
        //    dronFSM.ChangeState(DronState.Idle);
        //}
        }

    protected override void ExitState()
    {
        if (!IsServer) return;

        timerText.text = "";
        //dronLight.color = new Color(255,151,0,255);
        nav.SetDestination(dronFSM.transform.position);
    }
}
