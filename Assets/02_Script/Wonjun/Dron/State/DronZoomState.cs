using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronZoomState : DronStateRoot
{
    float zoomRange;
    float zoomtime = 0;
     Light droneLight;

    public DronZoomState(DronFSM controller, float zoomRange, Light dronLt) : base(controller)
    {
        this.zoomRange = zoomRange;
        this.droneLight = dronLt;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;
        zoomtime = 0;
        dronFSM.zoom = false;
        nav.isStopped = true;

        NetworkSoundManager.Play3DSound("DronHowling", dronFSM.transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);
    }

    protected override void UpdateState()
    {
        
        if (!IsServer) return;

        if (zoomtime >= 5f)
        {
            nav.isStopped = false;

            controller.ChangeState(DronState.Patrol);
        }
        else
        {
            zoomtime += Time.deltaTime;
            droneLight.innerSpotAngle = Mathf.Lerp(0, zoomRange, zoomtime / 5f);
            Collider targetPlayer = dronFSM.ViewingPlayer(Mathf.Lerp(5f, 10f, zoomtime / 5f), 0);
            if(targetPlayer != null)
            {
                dronFSM.targetPlayer = targetPlayer;
                Debug.Log("플레이어가 들어와서 상태 바뀜");
                dronFSM.zoom = true;
            }
        }
    }

    protected override void ExitState()
    {
        if (!IsServer) return;
        nav.isStopped = false;

        droneLight.innerSpotAngle = Mathf.Lerp(zoomRange, 0, zoomtime / 5f);
        dronFSM.ViewingPlayer(Mathf.Lerp(10f, 5f, 0.5f), 20);
        zoomtime = 0;
        dronFSM.zoom = false;
    }
}
