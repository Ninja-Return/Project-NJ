using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LightMonsterSencer : NetworkBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            if (other.TryGetComponent<PlayerController>(out var control))
            {

                if(control.OwnerClientId == NetworkManager.LocalClientId)
                {

                    control.isInsideSafetyRoom = true;

                }

            }

        }

    }


    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            if (other.TryGetComponent<PlayerController>(out var control))
            {

                if (control.OwnerClientId == NetworkManager.LocalClientId)
                {

                    control.isInsideSafetyRoom = false;

                }

            }

        }

    }

}
