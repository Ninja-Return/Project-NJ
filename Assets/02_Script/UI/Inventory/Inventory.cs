using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EnumList;
using Unity.Netcode;


public class Inventory : NetworkBehaviour
{

    public static Inventory Instance { get; private set; }

    private void Awake()
    {
        
        Instance = this;

    }

}