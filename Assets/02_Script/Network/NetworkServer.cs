using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

public class NetworkServer : IDisposable
{

    public NetworkServer(NetworkManager networkManager)
    {

        this.networkManager = networkManager;
        this.networkManager.ConnectionApprovalCallback += ApprovalChack;
        this.networkManager.OnServerStarted += OnServerReady;

    }


    private Dictionary<ulong, string> clientToAuthContainer = new();
    private Dictionary<string, UserData> authIdToUserDataContainer = new();
    private NetworkManager networkManager;

    public event Action<string, ulong> OnClientJoinEvent;
    public event Action<string, ulong> OnClientLeftEvent;

    private void ApprovalChack(NetworkManager.ConnectionApprovalRequest req, NetworkManager.ConnectionApprovalResponse res)
    {

        string json = Encoding.UTF8.GetString(req.Payload);
        var userData = JsonUtility.FromJson<UserData>(json);

        clientToAuthContainer.Add(req.ClientNetworkId, userData.authId);
        authIdToUserDataContainer.Add(userData.authId, userData);

        res.Approved = true;
        res.CreatePlayerObject = false;

        OnClientJoinEvent?.Invoke(userData.authId, req.ClientNetworkId);

    }

    private void OnServerReady()
    {

        networkManager.OnClientDisconnectCallback += OnClientDisconnect;

    }

    private void OnClientDisconnect(ulong clientId)
    {

        if (clientToAuthContainer.TryGetValue(clientId, out var authID))
        {
            clientToAuthContainer.Remove(clientId);
            authIdToUserDataContainer.Remove(authID);
            OnClientLeftEvent?.Invoke(authID, clientId);
        }

    }

    public UserData? GetUserDataByClientID(ulong clientID)
    {
        if (clientToAuthContainer.TryGetValue(clientID, out string authID))
        {

            if (authIdToUserDataContainer.TryGetValue(authID, out UserData data))
            {

                return data;

            }

        }

        return null;

    }

    public UserData? GetUserDataByAuthID(string authID)
    {
        if (authIdToUserDataContainer.TryGetValue(authID, out UserData data))
        {

            return data;

        }

        return null;
    }

    public void SetUserDataByClientId(ulong clientId, UserData userData)
    {

        if (clientToAuthContainer.TryGetValue(clientId, out string authID))
        {

            if (authIdToUserDataContainer.ContainsKey(authID))
            {

                authIdToUserDataContainer[authID] = userData;

            }

        }

    }

    public void Dispose()
    {

        if (networkManager == null) return;

        networkManager.ConnectionApprovalCallback -= ApprovalChack;
        networkManager.OnServerStarted -= OnServerReady;
        networkManager.OnClientDisconnectCallback -= OnClientDisconnect;

        if (networkManager.IsListening)
        {

            networkManager.Shutdown();

        }

    }

}
