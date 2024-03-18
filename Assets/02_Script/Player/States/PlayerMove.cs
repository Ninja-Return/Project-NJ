using FSM_System.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerStateRoot
{

    private PlayerController controller;

    public PlayerMove(PlayerController controller) : base(controller)
    {
    }
    protected override void EnterState()
    {
        controller = transform.root.GetComponent<PlayerController>();
    }

    protected override void UpdateState()
    {

        Move();

    }

    private void Move()
    {
        if (controller.isSittingDown)
        {
            var rigidVec = rigid.velocity;
            var inputVec = input.MoveVecter * data.SitSpeed.Value;

            rigidVec.x = inputVec.x;
            rigidVec.z = inputVec.y;

            rigidVec = transform.rotation * rigidVec;

            rigid.velocity = rigidVec;
        }
        else
        {
            var rigidVec = rigid.velocity;
            var inputVec = input.MoveVecter * data.MoveSpeed.Value;

            rigidVec.x = inputVec.x;
            rigidVec.z = inputVec.y;

            rigidVec = transform.rotation * rigidVec;

            rigid.velocity = rigidVec;
        }
        

    }

}
