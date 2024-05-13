using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class StartHard : NetworkBehaviour
{
    private void Start()
    {

        if (IsServer)
        {

            New_GameManager.Instance.OnHardEvent += HardStart;

        }

    }

    public void HardStart()
    {
        HardStartClientRpc();
    }

    [ClientRpc]
    private void HardStartClientRpc()
    {

        if(AlertText.Instance != null)
        {

            AlertText.Instance.HardStart();

        }

    }

}
