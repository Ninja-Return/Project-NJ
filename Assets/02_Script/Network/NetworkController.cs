using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkController : IDisposable
{

    public static NetworkController Instance { get; private set; }

    public VivoxController vivox { get; private set; }


    public NetworkController(string code) 
    {

        vivox = new VivoxController(NetworkManager.Singleton.LocalClientId, code);

    }

    public static void Init(string code)
    {

        Instance = new NetworkController(code);

    }

    public void Dispose()
    {

        Instance = null;

    }

}
