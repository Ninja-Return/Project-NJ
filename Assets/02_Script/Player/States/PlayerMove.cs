using FSM_System.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerStateRoot
{

    public PlayerMove(PlayerController controller) : base(controller)
    {
    }

    protected override void UpdateState()
    {

        Move();

    }

    private void Move()
    {

        var rigidVec = rigid.velocity;
        var inputVec = input.MoveVecter * data.MoveSpeed.Value;

        rigidVec.x = inputVec.x;
        rigidVec.z = inputVec.y;

        rigidVec = transform.rotation * rigidVec;

        rigid.velocity = rigidVec;

    }

}
