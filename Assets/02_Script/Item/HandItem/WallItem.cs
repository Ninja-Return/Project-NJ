using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WallItem : HandItemRoot
{
    [SerializeField] private NetworkObject Wall;
    public override void DoUse()
    {
        Debug.Log("�� ��ȯ");
        Wall = Instantiate(Wall, transform.position, Quaternion.identity);
        Wall.Spawn(true);
    }
}
