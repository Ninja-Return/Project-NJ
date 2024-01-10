using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerStateRoot
{
    public PlayerJump(PlayerController controller) : base(controller)
    {
    }

    protected override void EnterState()
    {

        input.OnJumpKeyPress += HandleJumpKeyPress;

    }

    protected override void ExitState()
    {

        input.OnJumpKeyPress -= HandleJumpKeyPress;

    }

    private void HandleJumpKeyPress()
    {

        if (!isGround) return;

        rigid.velocity += new Vector3(0, data.JumpPower.Value);

    }

}
