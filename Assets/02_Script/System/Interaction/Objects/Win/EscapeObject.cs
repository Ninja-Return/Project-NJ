using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeObject : InteractionObject
{

    protected override void DoInteraction()
    {

        WinSystem.Instance.WinServerRPC(EnumWinState.Player);

    }

}
