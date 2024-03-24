using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public enum PlayerRole
{

    Survivor,
    Mafia,
    New // 나중에 이름 정하기

}

public class PlayerRoleManager : NetworkBehaviour
{

    [SerializeField] private RoleUIController roleUI;
    [SerializeField] private bool debug;
    [SerializeField, Range(0, 1)] private float newRolePercentage = 1;

    private Dictionary<ulong, PlayerRole> roleContainer = new();

    public static PlayerRoleManager Instance { get; private set; }

    private void Awake()
    {

        Instance = this;

    }

    private void Start()
    {

        if (IsServer)
        {

            New_GameManager.Instance.OnGameStarted += HandleGameStarted;

        }

    }

    private void HandleGameStarted()
    {

        if(NetworkManager.ConnectedClients.Count > 3 || debug)
        {

            Debug.Log(123);
            SettingMafia();

        }

    }

    private void SettingMafia()
    {
        
        var clients = NetworkManager.ConnectedClients.Keys.ToList().GetRandomList(1000);

        {

            var mafiaId = clients[0];

            var param = new ClientRpcParams
            {

                Send = new ClientRpcSendParams
                {

                    TargetClientIds = new[] { mafiaId },

                }

            };

            SetRoleClientRPC(PlayerRole.Mafia, param);

            clients.Remove(mafiaId);

            roleContainer.Add(mafiaId, PlayerRole.Mafia);

        }

        if(Random.value <= newRolePercentage && clients.Count > 0)
        {

            var id = clients[0];

            var param = new ClientRpcParams
            {

                Send = new ClientRpcSendParams
                {

                    TargetClientIds = new[] { id },

                }

            };

            SetRoleClientRPC(PlayerRole.New, param);

            clients.Remove(id);

            roleContainer.Add(id, PlayerRole.New);

        }

        if(clients.Count  > 0)
        {

            var param = new ClientRpcParams
            {

                Send = new ClientRpcSendParams
                {

                    TargetClientIds = clients,

                }

            };

            SetRoleClientRPC(PlayerRole.Survivor, param);

        }

        foreach(var id in clients)
        {

            roleContainer.Add(id, PlayerRole.Survivor);

        }

    }

    [ClientRpc]
    public void SetRoleClientRPC(PlayerRole clientRole, ClientRpcParams rpcParams)
    {

        roleUI.SetRole(clientRole);    

    }

    public ulong? FindMafiaId()
    {

        foreach(var item in roleContainer)
        {

            if(item.Value == PlayerRole.Mafia)
            {

                return item.Key;

            }

        }

        return null;

    }

}
