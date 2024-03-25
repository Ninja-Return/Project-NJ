using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StartRoom : RoomData
{

    [ClientRpc]
    public void OpenAllClientRPC()
    {

        foreach(var item in closeDatas)
        {

            item.openObj.SetActive(true);
            item.closeObj.SetActive(false);

        }

    }

}
