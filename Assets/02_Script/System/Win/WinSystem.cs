using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EnumWinState
{

    None,
    Escape,
    Fail,

}

public class WinSystem : NetworkBehaviour
{

    public NetworkVariable<EnumWinState> winState { get; private set; } = new();
    public static WinSystem Instance;

    private void Awake()
    {

        Instance = this;

    }

    [ServerRpc(RequireOwnership = false)]
    public void WinServerRPC()
    {
        //ulong mafiaId = PlayerRoleManager.Instance.FindMafiaId().Value;
        //var data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(mafiaId);

        NetworkManager.SceneManager.LoadScene("Win", LoadSceneMode.Single);
        NetworkManager.SceneManager.UnloadScene(SceneManager.GetSceneByName("Room"));

        //PlayerPrefs.SetString("MafiaNickName", data.Value.nickName);

        //StartCoroutine(ReStart());

    }

    private IEnumerator ReStart()
    {

        yield return new WaitForSeconds(10);

        NetworkManager.SceneManager.LoadScene("WaitRoom", LoadSceneMode.Single);

    }

}