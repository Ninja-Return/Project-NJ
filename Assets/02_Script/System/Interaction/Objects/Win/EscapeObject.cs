using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeObject : InteractionObject
{

    protected override void DoInteraction()
    {

        PlayerManager.Instance.PlayerDie(EnumList.DeadType.Escape, NetworkManager.LocalClientId);


        UserData data = HostSingle.Instance.NetServer.GetUserDataByClientID(NetworkManager.LocalClientId).Value;
        data.clearTime = PlayerManager.Instance.PlayerTime;
        HostSingle.Instance.NetServer.SetUserDataByClientId(NetworkManager.LocalClientId, data);
    }

}
