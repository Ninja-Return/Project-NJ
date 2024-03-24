using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteItem : HandItemRoot
{
    public override void DoUse()
    {

        Inventory.Instance.Deleteltem();
        PlayerManager.Instance.localController.Active(false, true);
        KickUIController.Instance.OpenKickPanel();

    }

}
