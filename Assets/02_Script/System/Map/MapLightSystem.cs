using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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

        if(New_GameManager.Instance != null)
        {
            New_GameManager.Instance.OnHardEvent += HandleHardLight;
        }

    }

    private void HandleHardLight()
    {

        SetLightServerRPC(false);

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

        New_GameManager.Instance.IsLightOn.Value = value;
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

    public void SetLightEnable(bool value)
    {

        foreach (var item in lights)
        {

            item.Enable(value);

        }

    }

    public void SetLightColor(Color color)
    {

        foreach (var item in lights)
        {

            item.SetColor(color);

        }

    }



    public override void OnDestroy()
    {

        base.OnDestroy();

        Instance = null;

    }

}