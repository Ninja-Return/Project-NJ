using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrapDrop : NetworkBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float stunTime;

    private Rigidbody rigid;
    private bool isCatch;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!IsServer) return;

        RaderObj();
    }

    public void SetUp(Vector3 dir)
    {

        if (!IsServer) return;

        rigid.velocity = dir * 2;
        rigid.angularVelocity = UnityEngine.Random.insideUnitSphere * 30;

    }

    private void RaderObj()
    {
        if (isCatch) return;

        foreach (Collider col in Physics.OverlapSphere(transform.position, range))
        {
            if (col.TryGetComponent(out ICatchTrapInterface catchTrap))
            {
                isCatch = true;
                NetworkSoundManager.Play3DSound("TrapSound", transform.position, 0.1f, 15f);

                catchTrap.CaughtTrap(stunTime);

                NetworkObject.Despawn();
                //StartCoroutine(DestoryCor());
            }
        }
    }

    private IEnumerator DestoryCor()
    {
        yield return new WaitForSeconds(stunTime);

        NetworkObject.Despawn();
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

#endif
}
