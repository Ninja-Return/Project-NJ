using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AddGravity : NetworkBehaviour
{

    private GroundSencer groundSencer;
    private Rigidbody rigid;

    private void Awake()
    {
        
        groundSencer = GetComponentInChildren<GroundSencer>();
        rigid = GetComponent<Rigidbody>();

    }

    private void Update()
    {

        if (!groundSencer.IsGround)
        {

            rigid.velocity -= new Vector3(0, 9.81f) * Time.deltaTime * 2;

        }

    }

}
