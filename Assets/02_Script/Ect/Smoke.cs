using OccaSoftware.Buto.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.Netcode;
using UnityEngine;

public class Smoke : NetworkBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float destroyTime = 10f;

    private void Start()
    {

        if (!IsServer) return;

        NetworkSoundManager.Play3DSound("Smoke", transform.position, 0.1f, 20f);
        StartCoroutine(DestoryCo());

        StunMachine();

    }

    private void StunMachine()
    {
        var col = Physics.OverlapSphere(transform.position, range);

        foreach (var obj in col)
        {
            if (obj.TryGetComponent(out IMachineInterface machine))
            {
                machine.Stun(destroyTime);
            }
        }
    }

    private IEnumerator DestoryCo()
    {

        yield return new WaitForSeconds(destroyTime);

        NetworkObject.Despawn();

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
#endif
}
