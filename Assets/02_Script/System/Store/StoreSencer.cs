using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StoreSencer : NetworkBehaviour
{

    [SerializeField] private StoreUIController shopTrm;

    private void OnTriggerEnter(Collider other)
    {

        if (WaitRoomManager.Instance.IsRunningGame.Value == false) return;

        if (other.TryGetComponent<PlayerController>(out var pl))
        {

            shopTrm.StartSeq();

        }

    }



}
