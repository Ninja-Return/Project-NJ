using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronInPlayerTransition : DronTransitionRoot
{
    private float radius;
    private bool zoom;

    public DronInPlayerTransition(DronFSM controller, DronState nextState, float radius, bool zoom) : base(controller, nextState)
    {
        this.radius = radius;
        this.zoom = zoom;
    }

    protected override bool CheckTransition()
    {
        if (nav.pathPending) return false;

        Collider targetPlayer = dronFSM.ViewingPlayer(radius, 20);
        if (targetPlayer != null || zoom)
        {
            dronFSM.targetPlayer = targetPlayer;
            Debug.Log("�÷��̾ ���ͼ� ���� �ٲ�");
            return true;
        }

        return false;
    }
}
