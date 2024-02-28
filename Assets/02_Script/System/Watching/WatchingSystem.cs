using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class WatchingSystem : NetworkBehaviour
{

    public static WatchingSystem Instance;

    private ulong currentWatching;
    private bool isWatching;
    private List<PlayerController> alivePlayers = new();

    private void Awake()
    {

        Instance = this;

    }

    private void Start()
    {

        GameManager.Instance.alivePlayer.OnListChanged += HandleListChanged;

    }

    private void HandleListChanged(NetworkListEvent<ulong> changeEvent)
    {

        if (!isWatching) return;

        switch (changeEvent.Type) 
        {

            case NetworkListEvent<ulong>.EventType.Remove:
                {

                    alivePlayers.Remove(alivePlayers.Find(x => x.OwnerClientId == changeEvent.Value));

                    break;

                }



        }

        if(changeEvent.Value == currentWatching)
        {

            Watching(alivePlayers[0].OwnerClientId);

        }

    }

    public void Watching(ulong watch)
    {

        if(alivePlayers.Find(x => x.OwnerClientId == currentWatching) != null)
        {

            alivePlayers.Find(x => x.OwnerClientId == currentWatching).watchCam.Priority = -1;

        }

        alivePlayers.Find(x => x.OwnerClientId == watch).watchCam.Priority = 1000;
        currentWatching = watch;

    }

    public void StartWatching()
    {

        isWatching = true;

        alivePlayers = FindObjectsOfType<PlayerController>().ToList();

        Watching(alivePlayers[0].OwnerClientId);

    }

}
