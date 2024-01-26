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
    Mafia

}

public class PlayerRoleManager : NetworkBehaviour
{

    [SerializeField] private TMP_Text mafiaText;
    [SerializeField] private bool debug;

    public ulong mafiaClientId { get; private set; }

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
        
        var clients = NetworkManager.ConnectedClients.Keys.ToList();

        var idx = Random.Range(0, clients.Count);

        SetMafiaClientRPC(clients[idx]);

    }

    [ClientRpc]
    public void SetMafiaClientRPC(ulong mafiaClientId)
    {

        if(NetworkManager.LocalClientId == mafiaClientId)
        {

            TextSetUp("당신의 역할은 마피아 입니다", Color.red);

        }
        else
        {

            TextSetUp("당신의 역할은 생존자 입니다", Color.blue);

        }

        this.mafiaClientId = mafiaClientId;

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
