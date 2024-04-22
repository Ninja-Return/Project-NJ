using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ClearTimeManager : NetworkBehaviour
{
    public NetworkVariable<float> playerTime = new NetworkVariable<float>();
    public bool TimerStarted { get; set; } = false;
    private float time = 0;

    public static ClearTimeManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerTime.Value = time;
    }

    private void Update()
    {
        playerTime.Value = time;

        if (TimerStarted == true)
        {
            time += Time.deltaTime;
        }

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
