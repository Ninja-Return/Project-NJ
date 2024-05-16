using System;
using Unity.Netcode;
using UnityEngine;

public class New_GameManager : NetworkBehaviour
{

    [Header("Wait")]
    [SerializeField] private int waitDelay = 10;

    public NetworkVariable<bool> IsLightOn { get; set; } = new();
    public NetworkVariable<bool> GameStarted { get; private set; } = new();
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

        if (IsServer)
        {

            IsLightOn.Value = true;    

        }

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {

            GameToHardServerRPC();

        }

    }

    public void CheckGameEnd(int playerCount)
    {

        if (playerCount == 0)
        {

            //FadeManager.Instance.FadeOn();
            OnGameFinishedClientRPC();
            WinSystem.Instance.WinServerRPC();

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
        GameStarted.Value = true;

    }

    public void GameToHard()
    {

        OnHardEvent?.Invoke();

    }

    //µð¹ö±×
    [ServerRpc(RequireOwnership = false)]
    private void GameToHardServerRPC()
    {

        GameToHard();
    }

}
