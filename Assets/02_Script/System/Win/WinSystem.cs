using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EnumWinState
{

    None,
    Player,
    Mafia,

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
    public void WinServerRPC(EnumWinState winState)
    {
        ulong mafiaId = PlayerRoleManager.Instance.FindMafiaId().Value;
        var data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(mafiaId);

        NetworkManager.SceneManager.LoadScene("Win", LoadSceneMode.Single);

        PlayerPrefs.SetInt("WinState", (int)winState);
        PlayerPrefs.SetString("MafiaNickName", data.Value.nickName);

        StartCoroutine(ShutDown());

    }

    public override void OnNetworkDespawn()
    {

        base.OnNetworkDespawn();

        Destroy(gameObject);
        Instance = null;

    }

    private IEnumerator ShutDown()
    {

        yield return new WaitForSeconds(3);

        HostSingle.Instance.GameManager.ShutdownAsync();

    }

}
