using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ClearTimeManager : NetworkBehaviour
{
    public bool TimerStarted { get; set; } = false;

    public static ClearTimeManager Instance;

    private void Awake()
    {
        Instance = this;
    }


    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {

        }

    }
}
