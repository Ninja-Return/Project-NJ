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
        if(dronFSM.zoom)
        {
            Collider zoomPlayer = dronFSM.ViewingPlayer(20f, 0);
            if (zoomPlayer != null)
            {
                dronFSM.targetPlayer = zoomPlayer.GetComponent<PlayerController>(); ;
                Debug.Log("�� ���¿��� �÷��̾ ���ͼ� ���� �ٲ�");
                return true;
            }
        }

        Collider targetPlayer = dronFSM.ViewingPlayer(radius, 20);
        if (targetPlayer != null)
        {
            dronFSM.targetPlayer = targetPlayer.GetComponent<PlayerController>(); ;
            Debug.Log("�÷��̾ ���ͼ� ���� �ٲ�");
            return true;
        }
       

        return false;
    }
}
