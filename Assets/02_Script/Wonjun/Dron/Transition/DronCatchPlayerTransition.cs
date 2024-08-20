using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronCatchPlayerTransition : DronTransitionRoot
{
    private float radius;

    public DronCatchPlayerTransition(DronFSM controller, DronState nextState, float radius) : base(controller, nextState)
    {
        this.radius = radius;
    }

    protected override bool CheckTransition()
    {
        if (nav.pathPending) return false;

        if (dronFSM.targetPlayer == null) return false;
        Collider player = dronFSM.CirclePlayer(radius);

        if (player != null)
        {
            dronFSM.targetPlayer = player.GetComponent<PlayerController>();
            Debug.Log("플레이어가 들어와서 죽음");
            return true;
        }

        return false;
    }
}
