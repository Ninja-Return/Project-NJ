using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class DronZoomState : DronStateRoot
{
    float zoomRange;
    float zoomTime;
    Light droneLight;

    public DronZoomState(DronFSM controller, float zoomRange, Light dronLt, float zoomTime) : base(controller)
    {
        this.zoomRange = zoomRange;
        this.droneLight = dronLt;
        this.zoomTime = zoomTime;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;
        zoomTime = 3f;
        dronFSM.zoom = false;
        nav.isStopped = true;

        NetworkSoundManager.Play3DSound("DronHowling", dronFSM.transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (zoomTime <= 0f)
        {
            nav.isStopped = false;

            controller.ChangeState(DronState.Patrol);
        }
        else
        {
            zoomTime -= Time.deltaTime;
            droneLight.innerSpotAngle = Mathf.Lerp(zoomRange, 0, zoomTime / 3f);
            Collider targetPlayer = dronFSM.ViewingPlayer(Mathf.Lerp(10f, 5f, zoomTime / 3f), 0);
            if (targetPlayer != null)
            {
                dronFSM.targetPlayer = targetPlayer.GetComponent<PlayerController>();
                Vector3 playerPos = targetPlayer.transform.position;
                Debug.Log("플레이어가 들어와서 상태 바뀜");
                dronFSM.zoom = true;
            }
        }
    }

    protected override void ExitState()
    {
        if (!IsServer) return;
        nav.isStopped = false;

        droneLight.innerSpotAngle = Mathf.Lerp(0, zoomRange, zoomTime / 3f);
        dronFSM.ViewingPlayer(Mathf.Lerp(5f, 10f, 0.5f), 5);
        zoomTime = 3f; 
        dronFSM.zoom = false;
    }
}
