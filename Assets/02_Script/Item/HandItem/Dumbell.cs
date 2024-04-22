using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Dumbell : HandItemRoot
{
    [SerializeField] private NetworkObject dumbellPrefab;
    [SerializeField] private float firePower;
    [SerializeField] private float spawnDistance = .5f; // ������ ������ �Ÿ�

    public override void DoUse()
    {
        Vector3 forwardDirection = Camera.main.transform.forward;

        Debug.Log("���� �߻�");
        Vector3 spawnPosition = transform.position + forwardDirection * spawnDistance;
        NetworkObject newDumbell = Instantiate(dumbellPrefab, spawnPosition, Quaternion.identity);
        newDumbell.Spawn(true);


        Rigidbody dumbellRigid = newDumbell.GetComponent<Rigidbody>();
        dumbellRigid.AddForce(forwardDirection * firePower, ForceMode.Impulse);
    }
}
