using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MapLightSystem : NetworkBehaviour
{

    private List<MapLight> lights = new();

    public static MapLightSystem Instance { get; private set; }


    private void Awake()
    {
        
        Instance = this;

    }

    private void Start()
    {

        New_GameManager.Instance.OnHardEvent += HandleHardLight;

    }

    private void HandleHardLight()
    {

        SetLightClientRPC(false);

    }

    public void AddLight(MapLight mapLight)
    {

        lights.Add(mapLight);

    }

    public void EnableLight()
    {

        SetLightServerRPC(true);

    }

    public void DisableLight()
    {

        SetLightServerRPC(false);

    }

    [ServerRpc(RequireOwnership = false)]
    private void SetLightServerRPC(bool value)
    {

        SetLightClientRPC(value);

    }

    [ClientRpc]
    private void SetLightClientRPC(bool value)
    {

        SetLight(value);

    }

    private void SetLight(bool value)
    {

        foreach(var item in lights)
        {

            item.SetLight(value);

        }

    }

}