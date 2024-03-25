using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soda : HandItemRoot
{
    public override void DoUse()
    {

        PlayerManager.Instance.localController.AddSpeed(3, 3);

    }

}
