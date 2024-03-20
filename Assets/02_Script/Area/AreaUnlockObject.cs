using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaUnlockObject : InteractionObject
{

    [SerializeField] private string unlockText;

    protected override void DoInteraction()
    {


        NotificationSystem.Instance.Notification(unlockText);

        Despawn();

    }

}
