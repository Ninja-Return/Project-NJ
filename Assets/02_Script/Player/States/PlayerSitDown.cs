using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSitDown : PlayerStateRoot
{
    public PlayerSitDown(PlayerController controller) : base(controller)
    {
    }

    protected override void EnterState()
    {

        input.OnSitDownKeyPress += HandleSitDownKeyPress;

    }

    protected override void ExitState()
    {

        input.OnSitDownKeyPress -= HandleSitDownKeyPress;

    }

    private void HandleSitDownKeyPress()
    {

        Debug.Log("¾É¾Æ");

    }

}
