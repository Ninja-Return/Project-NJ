using System;
using Unity.Netcode;
using UnityEngine;

public class ColorManager : NetworkBehaviour
{

    [SerializeField] SkinnedMeshRenderer serverMesh;
    [SerializeField] SkinnedMeshRenderer clientMesh;
    [SerializeField] private Color[] colors;

    public override void OnNetworkSpawn()
    {

        base.OnNetworkSpawn();
        HostSingle.Instance.GameManager.OnPlayerConnect += HandleColor;

    }

    public override void OnNetworkDespawn()
    {

        base.OnNetworkDespawn();
        HostSingle.Instance.GameManager.OnPlayerConnect -= HandleColor;

    }

    private void HandleColor(string authId, ulong clientId)
    {

        GiveColor(clientId);

    }

    private void GiveColor(ulong clientId)
    {

        if (IsOwner)
        {

            Color plColor = colors[UnityEngine.Random.Range(0, colors.Length)];
            serverMesh.material.color = plColor;
            clientMesh.material.color = plColor;

        }

    }
}
