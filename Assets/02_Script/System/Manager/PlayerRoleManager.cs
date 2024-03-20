using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public enum PlayerRole
{

    Survivor,
    Mafia,
    New // 나중에 이름 정하기

}

public class PlayerRoleManager : NetworkBehaviour
{

    [SerializeField] private TMP_Text mafiaText;
    [SerializeField] private bool debug;
    [SerializeField, Range(0, 1)] private float newRolePercentage = 1;

    private Dictionary<ulong, PlayerRole> roleContainer = new();

    public static PlayerRoleManager Instance { get; private set; }

    private void Awake()
    {

        Instance = this;

    }

    private void Start()
    {
        if (HostSingle.Instance.GameManager.gameMode == GameMode.Single) return;
        if (IsServer)
        {

            GameManager.Instance.OnGameStarted += HandleGameStarted;

        }

    }

    private void HandleGameStarted()
    {

        if(NetworkManager.ConnectedClients.Count > 3 || debug)
        {

            SettingMafia();

        }

    }

    private void SettingMafia()
    {
        
        var clients = NetworkManager.ConnectedClients.Keys.ToList().GetRandomList(1000);

        {

            var mafiaId = clients[0];

            var param = new ClientRpcParams
            {

                Send = new ClientRpcSendParams
                {

                    TargetClientIds = new[] { mafiaId },

                }

            };

            SetRoleClientRPC(PlayerRole.Mafia, param);

            clients.Remove(mafiaId);

            roleContainer.Add(mafiaId, PlayerRole.Mafia);

        }

        if(Random.value <= newRolePercentage && clients.Count > 0)
        {

            var id = clients[0];

            var param = new ClientRpcParams
            {

                Send = new ClientRpcSendParams
                {

                    TargetClientIds = new[] { id },

                }

            };

            SetRoleClientRPC(PlayerRole.New, param);

            clients.Remove(id);

            roleContainer.Add(id, PlayerRole.New);

        }

        if(clients.Count  > 0)
        {

            var param = new ClientRpcParams
            {

                Send = new ClientRpcSendParams
                {

                    TargetClientIds = clients,

                }

            };

            SetRoleClientRPC(PlayerRole.Survivor, param);

        }

        foreach(var id in clients)
        {

            roleContainer.Add(id, PlayerRole.Survivor);

        }

    }

    [ClientRpc]
    public void SetRoleClientRPC(PlayerRole clientRole, ClientRpcParams rpcParams)
    {

        SoundManager.Play2DSound("GameStart");

        switch (clientRole)
        {
            case PlayerRole.Survivor:
                TextSetUp("당신의 역할은 생존자 입니다", Color.blue);
                break;
            case PlayerRole.Mafia:
                TextSetUp("당신의 역할은 마피아 입니다", Color.red);
                break;
            case PlayerRole.New:
                TextSetUp("당신의 역할은 ??? 입니다", Color.green);
                break;
        }

    }

    public void TextSetUp(string text, Color color)
    {

        mafiaText.color = color;
        mafiaText.text = text;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1);
        seq.Append(mafiaText.DOFade(0, 1.5f));

    }

    public ulong FindMafiaId()
    {

        foreach(var item in roleContainer)
        {

            if(item.Value == PlayerRole.Mafia)
            {

                return item.Key;

            }

        }

        return 0;

    }

}
