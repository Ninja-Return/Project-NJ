using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class New_GameManager : NetworkBehaviour
{

    [Header("Wait")]
    [SerializeField] private int waitDelay = 10;


    private int joinCount;


    #region Events

    public event Action<int> OnWaitTimeChanged;
    public event Action OnWaitEnd;
    public event Action OnGameStarted;
    public event Action OnGameFinished;

    #endregion

    #region EventServerOnly

    public event Action OnPlayerSpawnCall;
    public event Action OnItemSpawnCall;
    public event Action OnHardEvent;

    #endregion

    public static New_GameManager Instance { get; private set; }

    private void Awake()
    {

        Instance = this;

    }

    public override void OnNetworkSpawn()
    {

        if (IsClient)
        {

            JoinSceneServerRPC();

        }

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {

            GameToHard();

        }

    }

    public void CheckGameEnd(int playerCount, bool IsBreaken)
    {

        if (playerCount == 0)
        {

            //FadeManager.Instance.FadeOn();
            OnGameFinishedClientRPC();
            WinSystem.Instance.WinServerRPC(IsBreaken == true ? EnumWinState.Escape : EnumWinState.Fail);

        }

    }

    public void SettingCursorVisable(bool visable)
    {

        if (PlayerManager.Instance.IsDie)
        {

            visable = true;

        }

        Support.SettingCursorVisable(visable);

    }


    #region ClientRPC

    [ClientRpc]
    private void OnTimeChangeClientRPC(int t)
    {

        OnWaitTimeChanged?.Invoke(t);

    }

    [ClientRpc]
    private void OnWaitEndClientRPC()
    {

        OnWaitEnd?.Invoke();

    }

    [ClientRpc]
    private void OnGameStartClientRPC()
    {
        
        OnGameStarted?.Invoke();

    }

    [ClientRpc]
    private void OnGameFinishedClientRPC()
    {

        OnGameFinished?.Invoke();

    }

    #endregion

    #region ServerRPC

    [ServerRpc(RequireOwnership = false)]
    private void JoinSceneServerRPC()
    {

        joinCount++;

    }

    #endregion
    public void Spawning()
    {

        OnItemSpawnCall?.Invoke();

    }

    public void GameToHard()
    {

        OnHardEvent?.Invoke();
        Debug.Log("公攫啊 决没抄 老");

    }


}
