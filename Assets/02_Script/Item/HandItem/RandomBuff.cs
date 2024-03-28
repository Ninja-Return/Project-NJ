using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RandomBuff : HandItemRoot
{

    public override void DoUse()
    {

        int randNum = Random.Range(1, 3);
        Debug.Log(randNum);
        if (randNum == 1)
        {

            PlayerManager.Instance.localController.AddSpeed(-7, 5);

        }
        else if (randNum == 2)
        {

            PlayerManager.Instance.localController.AddSpeed(3, 3);

        }
        else if (randNum == 3)
        {

            PlayerManager.Instance.localController.AddSpeed(3, 10);

        }

    }

}
