using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Dumbell : HandItemRoot
{
    [SerializeField] private NetworkObject dumbellPrefab;
    [SerializeField] private float firePower;
    [SerializeField] private float spawnDistance = .5f; // 앞으로 스폰할 거리

    public override void DoUse()
    {
        Vector3 forwardDirection = Camera.main.transform.forward;

        Debug.Log("덤벨 발사");
        Vector3 spawnPosition = transform.position + forwardDirection * spawnDistance;
        NetworkObject newDumbell = Instantiate(dumbellPrefab, spawnPosition, Quaternion.identity);
        newDumbell.Spawn(true);


        Rigidbody dumbellRigid = newDumbell.GetComponent<Rigidbody>();
        dumbellRigid.AddForce(forwardDirection * firePower, ForceMode.Impulse);
    }
}
