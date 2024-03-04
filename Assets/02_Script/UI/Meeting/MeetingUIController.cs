using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MeetingUIController : MonoBehaviour
{

    [SerializeField] private TMP_Text phaseText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private MeetingChatUIController chattingPanel;
    [SerializeField] private MeetingProfile panelPrefab;
    [SerializeField] private Transform panelRoot;
    [SerializeField] private TMP_Text itemShowPlayerNameText;
    [SerializeField] private TMP_Text itemShowText;

    private List<MeetingProfile> profiles = new List<MeetingProfile>();

    public void MeetingStart()
    {
        
        for(int i = 0; i < panelRoot.childCount; i++)
        {

            Destroy(panelRoot.GetChild(i).gameObject);

        }

        profiles.Clear();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void UpdateTime(int time)
    {

        timeText.text = $"남은 시간 : {time}";

    }

    public void PhaseChange(int phase)
    {

        phaseText.text = phase == 0 ? "애착품 공개 투표" : "처형 투표";

    }

    public void SpawnPanel(ulong clientId, string userName, bool isOwner)
    {

        var panel = Instantiate(panelPrefab, panelRoot);

        panel.Setting(clientId, userName, isOwner);

        profiles.Add(panel);

    }

    public void ChattingOpen()
    {

        bool active = !chattingPanel.gameObject.activeSelf;

        chattingPanel.Init(active);
        chattingPanel.gameObject.SetActive(active);

    }

    public void OpenVote(RPCList<VoteData> list)
    {

        foreach(VoteData item in list)
        {

            profiles.Find(x => x.ownerClientId == item.clientId).OpenVote(item.voteCount);

        }

    }

    public void CloseVote()
    {

        foreach(var item in profiles)
        {

            item.CloseVote();

        }

    }

    public void ShowingItem(string playerName, string items)
    {

        itemShowPlayerNameText.text = playerName;
        itemShowText.text = items;

        Sequence seq = DOTween.Sequence();


        itemShowPlayerNameText.SetColorAlpha(0);
        itemShowText.SetColorAlpha(0);

        seq.Append(itemShowPlayerNameText.DOFade(1, 0.5f));
        seq.Join(itemShowText.DOFade(1, 0.5f));
        seq.AppendInterval(0.3f);
        seq.Append(itemShowPlayerNameText.DOFade(0, 0.5f));
        seq.Join(itemShowText.DOFade(0, 0.5f));

    }

}