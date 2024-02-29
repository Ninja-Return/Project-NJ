using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NotificationSystem : MonoBehaviour
{


    public static NotificationSystem Instance { get; private set; }

    private void Awake()
    {
        
        Instance = this;

    }

    [ServerRpc(RequireOwnership = false)]
    private void NotificationServerRPC(FixedString512Bytes message)
    {

        NotificationClientRPC(message);

    }

    [ClientRpc]
    private void NotificationClientRPC(FixedString512Bytes message) 
    {
    
        

    }

    public void Notification(string message)
    {

        NotificationServerRPC(message);

    }

}
