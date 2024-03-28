using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soda : HandItemRoot
{
    public override void DoUse()
    {

        NetworkSoundManager.Play3DSound("SodaOpen", transform.position, 0.01f, 15f);
        PlayerManager.Instance.localController.AddSpeed(3, 3);

    }

}
