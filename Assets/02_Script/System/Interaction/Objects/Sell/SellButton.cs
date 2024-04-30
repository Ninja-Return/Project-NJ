using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButton : InteractionObject
{

    [SerializeField] private SellSystem sell;

    protected override void DoInteraction()
    {

        if (WaitRoomManager.Instance.IsRunningGame.Value == false) return;
        NetworkSoundManager.Play3DSound("Sell", transform.position, 0.01f, 10f);
        sell.Sell(NetworkManager.LocalClientId);

    }

}
