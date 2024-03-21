using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DrawerOpen : NetworkBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {

        if (!IsServer) return;

        if (collision.transform.TryGetComponent<InteractionObject>(out var compo))
        {

            compo.NetworkObject.TrySetParent(gameObject);

        }

    }

}
