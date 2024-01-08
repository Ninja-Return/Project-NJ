using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientSingle : MonoBehaviour
{

    public ClientGameManager GameManager { get; private set; }

    private static ClientSingle instance;
    public static ClientSingle Instance
    {

        get
        {

            if (instance != null) return instance;

            instance = FindObjectOfType<ClientSingle>();

            if (instance == null)
            {

                Debug.LogError("?");

            }

            return instance;

        }

    }

    public void CreateClient()
    {

        GameManager = new ClientGameManager(NetworkManager.Singleton);

    }


}
