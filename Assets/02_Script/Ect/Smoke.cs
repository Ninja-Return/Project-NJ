using OccaSoftware.Buto.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.Netcode;
using UnityEngine;

public class Smoke : NetworkBehaviour
{

    [SerializeField] private float destroyTime = 10f;

    private void Start()
    {

        if (!IsServer) return;

        StartCoroutine(DestoryCo());

    }

    private IEnumerator DestoryCo()
    {

        yield return new WaitForSeconds(destroyTime);

        NetworkObject.Despawn();

    }

}
