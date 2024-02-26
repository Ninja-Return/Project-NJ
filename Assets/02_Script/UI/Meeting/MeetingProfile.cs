using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MeetingProfile : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private TMP_Text userNameText;
    [SerializeField] private GameObject chackMask;

    private ulong ownerClientId;

    public void Setting(ulong ownerClientId, string userName, bool isOwner)
    {

        this.ownerClientId = ownerClientId;

        userNameText.text = userName;
        userNameText.color = isOwner ? Color.yellow : Color.black;

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if(MeetingSystem.Instance.Vote(ownerClientId))
        {

            chackMask.SetActive(true);

        }

    }

}
