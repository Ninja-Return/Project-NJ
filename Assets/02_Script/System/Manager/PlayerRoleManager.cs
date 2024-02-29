using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    private void Start()
    {

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
        
        var clients = NetworkManager.ConnectedClients.Keys.ToList().GetRandomList(100);

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

    }

    [ClientRpc]
    public void SetRoleClientRPC(PlayerRole clientRole, ClientRpcParams rpcParams)
    {

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

}
