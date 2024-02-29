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

        GameManager.Instance.alivePlayer.OnListChanged += HandleAliveChanged;

        foreach (var player in GameManager.Instance.alivePlayer)
        {

            var p = Instantiate(alivePlayerPrefab, alivePanelRoot);

            p.Spawn(player);

            alivePlayers.Add(p);

        }

        foreach(var player in GameManager.Instance.alivePlayer)
        {

            Instantiate(diePlayerPrefab, diePlayerRoot);

        }

    }

    private void HandleAliveChanged(NetworkListEvent<ulong> changeEvent)
    {

        switch (changeEvent.Type)
        {

            case NetworkListEvent<ulong>.EventType.Remove:
                {

                    Instantiate(diePlayerPrefab, diePlayerRoot);

                    Destroy(alivePlayers.Find(x => x.clientId == changeEvent.Value).gameObject);

                    break;

                }

        }

    }

}
