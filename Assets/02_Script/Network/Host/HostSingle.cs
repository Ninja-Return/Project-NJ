using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HostSingle : MonoBehaviour
{

    private static HostSingle instance;
    public static HostSingle Instance
    {

        get
        {

            if (instance != null) return instance;

            instance = FindObjectOfType<HostSingle>();

            if (instance == null)
            {

                Debug.LogError("?");

            }

            return instance;

        }

    }

    public HostGameManager GameManager { get; private set; }

    public void CreateHost()
    {

        GameManager = new HostGameManager();

    }

}
