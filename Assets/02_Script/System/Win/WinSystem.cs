using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EnumWinState
{

    None,
    Player,
    Mafia,

}

public class WinSystem : NetworkBehaviour
{

    public NetworkVariable<EnumWinState> winState { get; private set; } = new();
    public static WinSystem Instance;

    private void Awake()
    {

        Instance = this;

    }

    [ServerRpc]
    public void WinServerRPC(EnumWinState winState)
    {



    }



}
