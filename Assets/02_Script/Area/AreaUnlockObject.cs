using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaUnlockObject : InteractionObject
{

    [SerializeField] private string unlockText;

    protected override void DoInteraction()
    {

        //나중에 조건 추가

        NotificationSystem.Instance.Notification(unlockText);

        Despawn();

    }

}
