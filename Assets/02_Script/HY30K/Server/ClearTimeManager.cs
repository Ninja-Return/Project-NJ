using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ClearTimeManager : NetworkBehaviour
{
    public NetworkVariable<int> playerTime = new NetworkVariable<int>();
    private int time = 0;

    private void Start()
    {
        playerTime.Value = time;
    }

    private void Update()
    {

        playerTime.Value = time;
        time = time + (int)Time.deltaTime;

    }

    public override void OnNetworkSpawn()
    {

        if (IsServer)
        {
            UserData? data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(OwnerClientId);

            playerTime.Value = data.Value.clearTime;
        }

    }
}
