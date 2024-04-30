using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Dumbell : HandItemRoot
{

    [SerializeField] private float speed;
    [SerializeField] private ThrowedDumBell dumbellObject;

    public override void DoUse()
    {

        if (NetworkManager.Singleton.IsServer)
        {

            NetworkSoundManager.Play3DSound("ItemThrow", transform.position, 0.1f, 10f);
            ThrowDumBell();

        }

    }

    private void ThrowDumBell()
    {

        var obj = Instantiate(dumbellObject, transform.root.position + new Vector3(0, 0.5f, 0) + transform.root.forward, Quaternion.identity);
        obj.NetworkObject.Spawn(true);
        obj.SetUp(transform.root.forward);

    }


}
