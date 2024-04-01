using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndObject : InteractionObject
{
    protected override void DoInteraction()
    {
        WinSystem.Instance.WinServerRPC(EnumWinState.Escape);
    }
}
