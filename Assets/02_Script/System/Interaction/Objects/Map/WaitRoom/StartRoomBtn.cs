using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartRoomBtn : InteractionObject
{

    protected override void DoInteraction()
    {

        NetworkManager.SceneManager.LoadScene("Room", LoadSceneMode.Additive);

    }

}
