using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WaitRoomManager : NetworkBehaviour
{

    [SerializeField] private PlayerController prefab;

    public NetworkVariable<bool> IsRunningGame;

    public static WaitRoomManager Instance;

    private void Awake()
    {
        
        Instance = this;
        IsRunningGame = new();

    }
    

}
