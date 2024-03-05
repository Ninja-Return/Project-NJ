using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AlivePlayerPanel : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private TMP_Text userNameText;

    public ulong clientId { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {

        WatchingSystem.Instance.Watching(clientId);

    }

    public void Spawn(ulong clientId, string userName)
    {

        this.clientId = clientId;
        userNameText.text = userName;

    }

}
