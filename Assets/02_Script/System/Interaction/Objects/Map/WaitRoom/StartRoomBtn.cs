using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartRoomBtn : InteractionObject
{
    protected override void DoInteraction()
    {

        if (!IsServer || WaitRoomManager.Instance.IsRunningGame.Value == true) return;

        WaitRoomManager.Instance.IsRunningGame.Value = true;

        WaitRoomManager.Instance.UnActivePanelServerRpc();

        NetworkManager.SceneManager.LoadScene("Room", LoadSceneMode.Additive);

    }

}
