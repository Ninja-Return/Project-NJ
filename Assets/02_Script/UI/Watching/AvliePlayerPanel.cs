using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AvliePlayerPanel : MonoBehaviour, IPointerDownHandler
{

    private ulong clientId;

    public void OnPointerDown(PointerEventData eventData)
    {

        WatchingSystem.Instance.Watching(clientId);

    }

    public void Spawn(ulong clientId)
    {

        this.clientId = clientId;

    }

}
