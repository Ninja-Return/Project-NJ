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
    [SerializeField] private GameObject voteObject;
    [SerializeField] private Transform voteParent;

    private ulong ownerClientId;

    public void Setting(ulong ownerClientId, string userName, bool isOwner)
    {

        this.ownerClientId = ownerClientId;

        userNameText.text = userName;
        userNameText.color = isOwner ? Color.yellow : Color.black;

    }

    public void OpenVote(int voteCount)
    {

        for(int i = 0; i < voteCount; i++)
        {

            Instantiate(voteObject, voteParent);

        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if(MeetingSystem.Instance.Vote(ownerClientId))
        {

            chackMask.SetActive(true);

        }

    }

}
