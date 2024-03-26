using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartRoomBtn : InteractionObject
{

    [SerializeField] private GameObject uiPanel1;
    [SerializeField] private GameObject uiPanel2;

    protected override void DoInteraction()
    {

        if (!IsServer || WaitRoomManager.Instance.IsRunningGame.Value == true) return;

        WaitRoomManager.Instance.IsRunningGame.Value = true;

        uiPanel1.SetActive(false);
        uiPanel2.SetActive(false);

        NetworkManager.SceneManager.LoadScene("Room", LoadSceneMode.Additive);

    }

}
