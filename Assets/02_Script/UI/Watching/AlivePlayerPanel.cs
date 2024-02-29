using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AlivePlayerPanel : MonoBehaviour, IPointerDownHandler
{

    public ulong clientId { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {

        WatchingSystem.Instance.Watching(clientId);

    }

    public void Spawn(ulong clientId)
    {

        this.clientId = clientId;

    }

}
