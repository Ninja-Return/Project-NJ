using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartRoomBtn : InteractionObject
{

    protected override void DoInteraction()
    {

        if (!IsServer || WaitRoomManager.Instance.IsRunningGame.Value == true) return;

        WaitRoomManager.Instance.IsRunningGame.Value = true;
        NetworkManager.SceneManager.LoadScene("Room", LoadSceneMode.Additive);

    }

}
