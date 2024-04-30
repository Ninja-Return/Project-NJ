using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ThrowedDumBell : NetworkBehaviour
{

    private Rigidbody rigid;

    private void Awake()
    {

        rigid = GetComponent<Rigidbody>();

    }

    public void SetUp(Vector3 dir)
    {

        rigid.velocity = dir * 7;
        rigid.angularVelocity = dir * 7;

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (!IsServer) return;

        if (collision.gameObject.layer == LayerMask.GetMask("Player"))
        {

            if (collision.transform.TryGetComponent<PlayerController>(out var compo))
            {

                compo.AddSpeedClientRPC(-3, 3);

            }

        }

        NetworkObject.Despawn();

    }

}
