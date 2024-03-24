using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButton : InteractionObject
{

    [SerializeField] private SellSystem sell;

    protected override void DoInteraction()
    {

        if (WaitRoomManager.Instance.IsRunningGame.Value == false) return;

        sell.Sell(NetworkManager.LocalClientId);

    }

}
