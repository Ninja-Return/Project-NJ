using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MeetingPanel : MonoBehaviour, IPointerDownHandler
{

    private TMP_Text userNameText;
    private ulong ownerClientId;

    private void Awake()
    {
        
        userNameText = GetComponentInChildren<TMP_Text>();

    }

    public void Setting(ulong ownerClientId, string userName, bool isOwner)
    {

        this.ownerClientId = ownerClientId;

        userNameText.text = userName;
        userNameText.color = isOwner ? Color.yellow : Color.black;

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        MeetingSystem.Instance.Vote(ownerClientId);

    }

}
