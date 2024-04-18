using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronChaseState : DronStateRoot
{
    private float radius;
    private float speed;
    private float lazerTime;
    private float stopTime;
    private bool lazer;
    private bool razerCheck;

    public DronChaseState(DronFSM controller, float radius, float speed, float lazerTime, float stopTime) : base(controller)
    {
        this.radius = radius;
        this.speed = speed;
        this.lazerTime = lazerTime;
        this.stopTime = stopTime;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;
        Debug.Log("chase들어옴");
        NetworkSoundManager.Play3DSound("DronBite", transform.position, 0.1f, 40f, SoundType.SFX, AudioRolloffMode.Linear);


        nav.speed = speed;
        lazer = true;
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        //Vector3 playerPos = monsterFSM.targetPlayer.transform.position;
        //nav.SetDestination(playerPos);

        Collider player = dronFSM.CirclePlayer(radius);

        if (player != null)
        {
            dronFSM.targetPlayer = player;

            Vector3 playerPos = dronFSM.targetPlayer.transform.position;
            nav.SetDestination(playerPos);
            if (lazer && !razerCheck)
            {

                razerCheck = true;
                // 시작 시간마다 코루틴 호출
                Debug.Log("멈춰");
                StartCoroutine(PlayerStopLazerCoroutine());

            }
            
        }
        else
        {
            dronFSM.ChangeState(DronState.Chase);

        }
    }


    #region 플레이어 로직

    /// <summary>
    /// 플레이어 멈추기 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerStopLazerCoroutine()
    {
        while (lazer)
        {
            Debug.Log("플레이어 멈추게해", dronFSM.targetPlayer);
            dronFSM.targetPlayer.GetComponent<PlayerController>().Data.MoveSpeed.SetValue(0f);
            yield return new WaitForSeconds(stopTime);
            Debug.Log("다시 움직여", dronFSM.targetPlayer);
            dronFSM.targetPlayer.GetComponent<PlayerController>().Data.MoveSpeed.SetValue(5f);
            yield return new WaitForSeconds(lazerTime);
        }

        razerCheck = false;

    }
    #endregion

    protected override void ExitState()
    {
        if (!IsServer) return;

        lazer = false;
        nav.SetDestination(dronFSM.transform.position);
    }
}
