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

        NetworkSoundManager.Play3DSound("Smoke", transform.position, 0.1f, 20f);
        StartCoroutine(DestoryCo());

    }

    private IEnumerator DestoryCo()
    {

        yield return new WaitForSeconds(destroyTime);

        NetworkObject.Despawn();

    }

}
