using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Dumbell : HandItemRoot
{
    [SerializeField] private NetworkObject Dumbellprefab;

    public override void DoUse()
    {
        Debug.Log("�� ��ȯ");
        Dumbellprefab = Instantiate(Dumbellprefab, transform.position, Quaternion.identity);
        Dumbellprefab.Spawn(true);
    }
}
