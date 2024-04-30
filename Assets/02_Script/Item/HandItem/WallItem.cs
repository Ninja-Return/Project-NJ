using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WallItem : HandItemRoot
{
    [SerializeField] private NetworkObject Wall;
    public override void DoUse()
    {

        NetworkSoundManager.Play3DSound("WallBuild", transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);
        Wall = Instantiate(Wall, transform.position, Quaternion.identity);
        Wall.Spawn(true);

    }
}
