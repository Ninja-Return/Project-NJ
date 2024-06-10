using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRaderState : TurretStateRoot
{
    private float delay;
    private float currentTime;

    public TurretRaderState(TurretFSM controller, float delay) : base(controller)
    {
        this.delay = delay;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        NetworkSoundManager.Play3DSound("TurretAlarm", turretFSM.transform.position, 0.1f, 40f, SoundType.SFX, AudioRolloffMode.Linear);

        turretFSM.lineRenderer.enabled = true;
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        if (currentTime >= delay)
        {
            controller.ChangeState(TurretState.Fire);
        }
        else
        {
            currentTime += Time.deltaTime;

            Vector3 dir = (turretFSM.playerTrs.position - turretFSM.headTrs.position).normalized;
            turretFSM.headTrs.rotation = Quaternion.LookRotation(dir);

            turretFSM.lineRenderer.SetPosition(0, turretFSM.fireTrs.position);
            turretFSM.lineRenderer.SetPosition(1, turretFSM.playerTrs.position);
        }
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        turretFSM.lineRenderer.enabled = false; 
        currentTime = 0f;
    }
}
