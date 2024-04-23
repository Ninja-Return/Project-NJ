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

    private Dictionary<string, Transform> tpPos = new();

    #region Events

    public event Action<int> OnWaitTimeChanged;
    public event Action OnWaitEnd;
    public event Action OnGameStarted;
    public event Action OnGameFinished;

    #endregion

    #region EventServerOnly

    public event Action OnPlayerSpawnCall;
    public event Action OnItemSpawnCall;

    #endregion

    public static New_GameManager Instance { get; private set; }

    private void Awake()
    {

        Instance = this;

    }

    private void Start()
    {

       //if (!IsServer) return;
       //
       //StartCoroutine(StartLogicCo());

    }

    public override void OnNetworkSpawn()
    {

        if (IsClient)
        {

            JoinSceneServerRPC();

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

    public void AddPos(string str, Transform pos)
    {

        tpPos.Add(str, pos);

    }

    public void Spawning()
    {

        OnItemSpawnCall?.Invoke();

    }

    #region Coroutine

    private IEnumerator StartLogicCo()
    {

        yield return new WaitUntil(() => joinCount == NetworkManager.ConnectedClients.Count);

        yield return StartCoroutine(WaitLogicCo());

        OnWaitEndClientRPC();

        yield return null;



        yield return null;

        OnGameStartClientRPC();

    }

    private IEnumerator WaitLogicCo()
    {

        for (int i = waitDelay; i > 0; i--)
        {

            yield return new WaitForSeconds(1);
            OnTimeChangeClientRPC(i);

        }

        OnTimeChangeClientRPC(-1);

    }



    #endregion

}
