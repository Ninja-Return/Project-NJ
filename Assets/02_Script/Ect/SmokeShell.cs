using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SmokeShell : NetworkBehaviour
{

    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private NetworkObject smokeObject;

    private Rigidbody rigid;

    private void Awake()
    {
        
        rigid = GetComponent<Rigidbody>();

    }

    public void SetUp(Vector3 dir)
    {

        if (!IsServer) return;

        rigid.velocity = dir * 7;
        rigid.angularVelocity = UnityEngine.Random.insideUnitSphere * 30;

        StartCoroutine(DestoryCo());

    }

    private IEnumerator DestoryCo()
    {

        yield return new WaitForSeconds(destroyTime);

        Instantiate(smokeObject, transform.position, Quaternion.identity).Spawn(true);

        NetworkObject.Despawn();

    }

}