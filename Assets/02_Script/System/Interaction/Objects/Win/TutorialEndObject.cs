using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndObject : InteractionObject
{
    protected override void DoInteraction()
    {
        var data = HostSingle.Instance.NetServer.GetUserDataByClientID(OwnerClientId).Value;
        data.isBreak = true;
        HostSingle.Instance.NetServer.SetUserDataByClientId(OwnerClientId, data);

        WinSystem.Instance.WinServerRPC(EnumWinState.Escape);
    }
}
