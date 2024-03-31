using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{

    public ClientGameManager(NetworkManager networkManager)
    {

        this.networkManager = networkManager;

    }

    private void HandleClientDisconnect(bool obj)
    {

        networkManager.OnClientStopped -= HandleClientDisconnect;
        Disconnect();

    }

    private NetworkManager networkManager;
    private JoinAllocation allocation;

    public void Disconnect()
    {

        if (networkManager.IsConnectedClient)
        {

            networkManager.Shutdown();

        }

        if(NetworkController.Instance != null)
        {

            NetworkController.Instance.Dispose();

        }


        SceneManager.LoadScene(SceneList.LobbySelectScene);

    }

    public async Task StartClientAsync(string joinCode, UserData data)
    {

        try
        {

            allocation = await Relay.Instance.JoinAllocationAsync(joinCode);

        }
        catch(System.Exception ex)
        {

            Debug.LogException(ex);
            return;

        }

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        var relayServerData = new RelayServerData(allocation, "dtls");

        transport.SetRelayServerData(relayServerData);

        string json = JsonUtility.ToJson(data);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(json);
        NetworkManager.Singleton.StartClient();

        NetworkManager.Singleton.OnClientStopped += HandleClientDisconnect;

        NetworkController.Init(joinCode);

    }

}
