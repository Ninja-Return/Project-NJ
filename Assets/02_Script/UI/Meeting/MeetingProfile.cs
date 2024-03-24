using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MeetingProfile : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private TMP_Text userNameText;
    [SerializeField] private Image mainPanel;
    [SerializeField] private Button voteBtn;
    [SerializeField] private GameObject chackMask;
    [SerializeField] private GameObject voteObject;
    [SerializeField] private Transform voteParent;

    public ulong ownerClientId { get; private set; }

    public void Setting(ulong ownerClientId, string userName, bool isOwner, bool isVote = true)
    {

        this.ownerClientId = ownerClientId;

        userNameText.text = userName;
        userNameText.color = isOwner ? Color.yellow : Color.white;

        voteBtn.enabled = isVote;

    }

    public void ColorChange(Color panelColor)
    {
        mainPanel.color = panelColor;
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

    public void CloseVote()
    {

        for(int i = 0; i < voteParent.childCount; i++) 
        {
            
            Destroy(voteParent.GetChild(i).gameObject);
        
        }

        chackMask.SetActive(false);

    }

}
