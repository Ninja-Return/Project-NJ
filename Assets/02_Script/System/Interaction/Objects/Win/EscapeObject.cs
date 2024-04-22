using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeObject : InteractionObject
{

    protected override void DoInteraction()
    {

        ClearTimeManager.Instance.TimerStarted = false;
        PlayerManager.Instance.PlayerDie(EnumList.DeadType.Escape, NetworkManager.LocalClientId);

    }

}
