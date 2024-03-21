using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WatchingUIController : MonoBehaviour
{

    [SerializeField] private AlivePlayerPanel alivePlayerPrefab;
    [SerializeField] private DiePlayerPanel diePlayerPrefab;
    [SerializeField] private Transform alivePanelRoot;
    [SerializeField] private Transform diePlayerRoot;

    private List<AlivePlayerPanel> alivePlayers = new();

    public void Init()
    {

        PlayerManager.Instance.alivePlayer.OnListChanged += HandleAliveChanged;

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

                    Instantiate(diePlayerPrefab, diePlayerRoot);

                    Destroy(alivePlayers.Find(x => x.clientId == changeEvent.Value.clientId).gameObject);

                    break;

                }

        }

    }

}
