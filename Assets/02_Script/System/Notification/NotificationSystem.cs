using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class NotificationSystem : NetworkBehaviour
{

    [SerializeField] private NotificationUIController notificationUIController;

    public static NotificationSystem Instance 
    {
        get
        {

            if(instance == null)
            {

                instance = Object.FindObjectOfType<NotificationSystem>();

            }

            return instance;

        }
    }
    private static NotificationSystem instance;

    public override void OnNetworkSpawn()
    {

        instance = this;

    }

    [ServerRpc(RequireOwnership = false)]
    private void NotificationServerRPC(FixedString512Bytes message)
    {

        NotificationClientRPC(message);

    }

    [ClientRpc]
    private void NotificationClientRPC(FixedString512Bytes message) 
    {

        notificationUIController.Notification(message.ToString());

    }

    public void Notification(string message)
    {

        NotificationServerRPC(message);

    }

}
