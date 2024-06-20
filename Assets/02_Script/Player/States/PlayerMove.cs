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

        var rigidVec = rigid.velocity;
        var speed = controller.isSittingDown ? data.SitSpeed.Value : data.MoveSpeed.Value;
        var inputVec = input.MoveVecter * speed;

        rigidVec.x = inputVec.x;
        rigidVec.z = inputVec.y;

        rigidVec = transform.rotation * rigidVec;

        rigid.velocity = rigidVec;

    }

}
