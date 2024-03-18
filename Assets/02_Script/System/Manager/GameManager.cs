using EnumList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct LiveData : INetworkSerializable, IEquatable<LiveData>
{

    public ulong clientId;
    public FixedString64Bytes name;

    public bool Equals(LiveData other)
    {

        return other.clientId == clientId;

    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {

        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref name);

    }

}

public class GameManager : NetworkBehaviour
{

    [SerializeField] private NetworkObject player;
    [SerializeField] private bool debug;
    [SerializeField] private DeathUI deathUI;
    [SerializeField] private List<Transform> trms;
    [SerializeField] private TMP_Text startText;
    [SerializeField] private GameObject recipeUI;

    private List<PlayerController> players = new();
    public PlayerController clientPlayer { get; private set; }

    public static GameManager Instance;
    public NetworkList<LiveData> alivePlayer { get; private set; }
    public NetworkList<LiveData> diePlayer { get; private set; }



    public event Action OnGameStarted;
    public event Action OnGameStartCallEnd;
    public bool PlayerMoveable { get; private set; } = true;
    public bool isDie { get; private set; }


    private void Awake()
    {

        alivePlayer = new();
        diePlayer = new();

        Instance = this;

    }

    private IEnumerator Start()
    {

        for(int i = 10; i > 0; i--)
        {

            SetTextClientRPC(i);
            yield return new WaitForSeconds(1);

        }

        SetTextClientRPC(-1);

        yield return new WaitForSeconds(1);

        OnGameStarted?.Invoke();

        yield return null;

        OnGameStartCallEnd?.Invoke();

        if (IsServer)
        {

            StartGame();
            HostSingle.Instance.GameManager.OnPlayerConnect += HandlePlayerConnect;

            yield return new WaitForSeconds(1);

            var param = new ClientRpcParams
            {

                Send = new ClientRpcSendParams
                {

                    TargetClientIds = new[] { PlayerRoleManager.Instance.FindMafiaId() },

                }

            };

            players.Find(x => x.OwnerClientId == PlayerRoleManager.Instance.FindMafiaId()).SetMafiaClientRPC(param);

        }

        yield return new WaitForSeconds(0.5f);

        SetLocalPlayerClientRPC();

        
    }

    [ClientRpc]
    private void SetLocalPlayerClientRPC()
    {

        foreach(var item in FindObjectsOfType<PlayerController>())
        {

            if(item.OwnerClientId == NetworkManager.LocalClientId)
            {

                clientPlayer = item; 
                break;

            }

        }

    }

    private void HandlePlayerConnect(string authId, ulong clientId)
    {

        SpawnPlayer(clientId);

    }

    private void StartGame()
    {

        foreach(var id in NetworkManager.ConnectedClientsIds)
        {

            SpawnPlayer(id);

        }

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {

            recipeUI.SetActive(!recipeUI.activeSelf);

        }

    }

    public override void OnDestroy()
    {
        
        base.OnDestroy();

        Instance = null;

    }

    public void SpawnPlayer(ulong clientId)
    {

        var spawnTrm = trms.GetRandomListObject();
        trms.Remove(spawnTrm);

        var pl = Instantiate(player, spawnTrm.position, Quaternion.identity).GetComponent<PlayerController>();
        pl.NetworkObject.SpawnWithOwnership(clientId, true);

        var data = HostSingle.Instance.NetServer.GetUserDataByClientID(pl.OwnerClientId).Value;

        players.Add(pl);
        alivePlayer.Add(new LiveData { clientId = pl.OwnerClientId, name = data.nickName });

    }

    [ClientRpc]
    public void PlayerMoveableChangeClientRPC(bool value)
    {

        var obj = FindObjectsOfType<PlayerController>().ToList().Find(x => x.IsOwner);

        if(obj != null)
        {

            obj.Active(value);

        }

    }

    public void PlayerDie(DeadType type, ulong clientId)
    {

        players.Find(x => x.OwnerClientId == clientId).NetworkObject.Despawn();

        var data = HostSingle.Instance.NetServer.GetUserDataByClientID(clientId).Value;
        data.isDie = true;

        HostSingle.Instance.NetServer.SetUserDataByClientId(clientId, data);

        var param = new ClientRpcParams
        {

            Send = new ClientRpcSendParams
            {

                TargetClientIds = new[] { clientId },

            }

        };

        var live = new LiveData();

        foreach(var item in alivePlayer)
        {

            if(item.clientId == clientId)
            {

                live = item;
                alivePlayer.Remove(item);
                break;

            }

        }

        diePlayer.Add(live);

        if (!debug)
        {

            var id = PlayerRoleManager.Instance.FindMafiaId();

            if(alivePlayer.Count <= 2 && alivePlayer.Find(x => x.clientId == id).clientId == id)
            {

                WinSystem.Instance.WinServerRPC(EnumWinState.Mafia);

            }

        }

        PlayerDieClientRPC(type, param);

    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerDieServerRPC(DeadType type, ulong id)
    {

        PlayerDie(type, id);

    }

    [ClientRpc]
    private void PlayerDieClientRPC(DeadType type, ClientRpcParams param)
    {

        
        deathUI.gameObject.SetActive(true);
        deathUI.PopupDeathUI(type);
        isDie = true;

        NetworkController.Instance.vivox.Leave3DChannel();

    }

    public void SettingCursorVisable(bool visable)
    {

        if (isDie)
        {

            visable = true;

        }

        Cursor.lockState = visable ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visable;

    }

    [ClientRpc]
    private void SetTextClientRPC(int t)
    {

        startText.text = $"시작까지 : {t}";

        if (t == -1) startText.text = "";

    }

}
