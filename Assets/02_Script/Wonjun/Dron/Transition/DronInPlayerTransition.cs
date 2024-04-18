using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronInPlayerTransition : DronTransitionRoot
{
    private float radius;

    public DronInPlayerTransition(DronFSM controller, DronState nextState, float radius) : base(controller, nextState)
    {
        this.radius = radius;
    }

    protected override bool CheckTransition()
    {
        if (nav.pathPending) return false;

        Collider targetPlayer = dronFSM.ViewingPlayer(radius);
        if (targetPlayer != null)
        {
            dronFSM.targetPlayer = targetPlayer;
            Debug.Log("�÷��̾ ���ͼ� ���� �ٲ�");
            return true;
        }

        return false;
    }
}
