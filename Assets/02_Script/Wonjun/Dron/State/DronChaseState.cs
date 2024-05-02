using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronChaseState : DronStateRoot
{
    private float radius;
    private LineRenderer lazerLine;
    private float speed;
    private float lazerTime;
    private float stopTime;
    private bool lazer;
    private bool razerCheck;

    public DronChaseState(DronFSM controller, float radius, float speed, float lazerTime, float stopTime, LineRenderer lazerLine) : base(controller)
    {
        this.radius = radius;
        this.speed = speed;
        this.lazerTime = lazerTime;
        this.stopTime = stopTime;
        this.lazerLine = lazerLine;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;
        NetworkSoundManager.Play3DSound("DronBite", transform.position, 0.1f, 40f, SoundType.SFX, AudioRolloffMode.Linear);
        Debug.Log("chase����");
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
                // ���� �ð����� �ڷ�ƾ ȣ��
                Debug.Log("����");
                StartCoroutine(PlayerStopLazerCoroutine());

            }
            
        }
        else
        {
            dronFSM.ChangeState(DronState.Idle);
        }
    }


    #region �÷��̾� ����

    /// <summary>
    /// �÷��̾� ���߱� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerStopLazerCoroutine()
    {
        while (lazer)
        {
            lazerLine.SetPosition(0, dronFSM.headTrs.position);
            lazerLine.SetPosition(1, dronFSM.targetPlayer.transform.position);
            Debug.Log("�÷��̾� ���߰���", dronFSM.targetPlayer);
            dronFSM.targetPlayer.GetComponent<PlayerController>().Data.MoveSpeed.SetValue(0f);
            yield return new WaitForSeconds(0.5f);
            lazerLine.SetPosition(1, dronFSM.headTrs.position);
            lazerLine.SetPosition(0, dronFSM.headTrs.position);
            yield return new WaitForSeconds(stopTime-1);
            Debug.Log("�ٽ� ������", dronFSM.targetPlayer);
            
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
