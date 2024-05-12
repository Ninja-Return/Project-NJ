using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class WatchingUIController : MonoBehaviour
{

    [SerializeField] private AlivePlayerPanel alivePlayerPrefab;
    [SerializeField] private DiePlayerPanel diePlayerPrefab;
    [SerializeField] private Transform alivePanelRoot;
    [SerializeField] private Transform diePlayerRoot;
    [SerializeField] private Transform clearPanel;

    private TMP_Text clearText;

    private List<AlivePlayerPanel> alivePlayers = new();

    public void Init()
    {

        PlayerManager.Instance.alivePlayer.OnListChanged += HandleAliveChanged;

        clearText = clearPanel.GetComponentInChildren<TMP_Text>();

        //var data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(PlayerManager.Instance.OwnerClientId);
        //if (data.Value.isBreak)
        //{
        //    clearText.text = "Å»ÃâÇÏ¼Ì½À´Ï´Ù";
        //}
        //else
        //{
        //    clearText.text = "Á×À¸¼Ì½À´Ï´Ù";
        //}

        //StartCoroutine(StartClear());

        foreach (var player in PlayerManager.Instance.alivePlayer)
        {

            var p = Instantiate(alivePlayerPrefab, alivePanelRoot);

            p.Spawn(player.clientId, player.name.ToString());

            alivePlayers.Add(p);

        }

        foreach(var player in PlayerManager.Instance.diePlayer)
        {

            var p = Instantiate(diePlayerPrefab, diePlayerRoot);
            p.Spawn(player.name.ToString());

        }

    }

    private void HandleAliveChanged(NetworkListEvent<LiveData> changeEvent)
    {

        switch (changeEvent.Type)
        {

            case NetworkListEvent<LiveData>.EventType.Remove:
                {

                    var p = Instantiate(diePlayerPrefab, diePlayerRoot);
                    p.Spawn(changeEvent.Value.name.ToString());

                    Destroy(alivePlayers.Find(x => x.clientId == changeEvent.Value.clientId).gameObject);

                    break;

                }

        }

    }

    private IEnumerator StartClear()
    {

        clearPanel.transform.TVEffect(true);

        yield return new WaitForSeconds(2f);

        clearPanel.transform.TVEffect(false);

    }

}
