using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerDarkness : NetworkBehaviour
{
    [SerializeField] private GameObject darkImage;

    readonly float darkTime = 0.05f;

    public void Bliend(ulong clientId)
    {
        BliendClientRpc(clientId);
    }

    [ClientRpc]
    private void BliendClientRpc(ulong clientId)
    {
        if (clientId != NetworkManager.LocalClientId) return;

        StartCoroutine(BliendCor());
    }

    private IEnumerator BliendCor()
    {
        darkImage.SetActive(true);

        yield return new WaitForSeconds(darkTime);

        darkImage.SetActive(false);
    }
}
