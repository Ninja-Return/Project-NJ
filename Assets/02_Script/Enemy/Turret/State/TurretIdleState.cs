using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurretIdleState : TurretStateRoot
{
    private float angle;

    readonly private float spinSpeed = 1.2f;
    readonly private float spinDelay = 1f;

    Coroutine spinCor;

    public TurretIdleState(TurretFSM controller, float angle) : base(controller)
    {
        this.angle = angle;
    }

    protected override void EnterState()
    {
        if (!IsServer) return;

        spinCor = StartCoroutine(SpinTurret());
    }

    protected override void UpdateState()
    {
        if (!IsServer) return;

        Collider targetPlayer = turretFSM.ViewingPlayer();
        if (targetPlayer != null)
        {
            turretFSM.playerTrs = targetPlayer.transform;
            controller.ChangeState(TurretState.Rader);
        }
    }

    protected override void ExitState()
    {
        if (!IsServer) return;

        if (spinCor != null)
        {
            StopCoroutine(spinCor);
        }
    }

    private IEnumerator SpinTurret()
    {
        float delay = spinSpeed + spinDelay;

        while (true)
        {
            turretFSM.headTrs.DORotate(new Vector3(0f, angle, 0f), spinSpeed);
            yield return new WaitForSeconds(delay);

            turretFSM.headTrs.DORotate(new Vector3(0f, -angle, 0f), spinSpeed);
            yield return new WaitForSeconds(delay);
        }
    }
}
