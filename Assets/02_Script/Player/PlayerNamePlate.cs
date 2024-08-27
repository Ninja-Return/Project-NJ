using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerNamePlate : NetworkBehaviour
{
    [SerializeField] private TMP_Text namePlate;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {

            foreach (var item in NetworkManager.ConnectedClientsIds)
            {
                UserData data = HostSingle.Instance.NetServer.GetUserDataByClientID(item).Value;
                SettingName(item, data.nickName);
            }
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            namePlate.gameObject.SetActive(false);
        }
        else
        {
            if (PlayerManager.Instance.localController != null)
            {
                namePlate.transform.LookAt(PlayerManager.Instance.localController.transform);
            }
        }
    }

    private void SettingName(ulong clientId, string name)
    {
        SettingNameClientRpc(clientId, name);
    }

    [ClientRpc]
    private void SettingNameClientRpc(ulong clientId, string name)
    {
        Debug.Log($"{OwnerClientId}, {clientId}, {name}");
        if (clientId != OwnerClientId) return;

        namePlate.text = name;
    }
}
