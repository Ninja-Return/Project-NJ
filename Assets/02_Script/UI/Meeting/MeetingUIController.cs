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
    [SerializeField] private Transform endPlayerPanel;

    private List<MeetingProfile> profiles = new List<MeetingProfile>();

    public void MeetingStart()
    {

        if (PlayerManager.Instance.IsDie) return;

        for (int i = 0; i < panelRoot.childCount; i++)
        {

            Destroy(panelRoot.GetChild(i).gameObject);

        }

        profiles.Clear();

        New_GameManager.Instance.SettingCursorVisable(true);
        chattingPanel.Init();

    }

    public void UpdateTime(int time)
    {

        if (PlayerManager.Instance.IsDie) return;

        timeText.text = $"남은 시간 : {time}";

    }

    public void PhaseChange(int phase)
    {

        if (PlayerManager.Instance.IsDie) return;

        phaseText.text = phase == 0 ? "애착품 공개 투표" : "처형 투표";

    }

    public void SpawnPanel(ulong clientId, string userName, bool isOwner)
    {

        if (PlayerManager.Instance.IsDie) return;

        var panel = Instantiate(panelPrefab, panelRoot);

        panel.Setting(clientId, userName, isOwner);

        profiles.Add(panel);

    }


    public void OpenVote(RPCList<VoteData> list)
    {

        if (PlayerManager.Instance.IsDie) return;

        foreach (VoteData item in list)
        {

            profiles.Find(x => x.ownerClientId == item.clientId).OpenVote(item.voteCount);

        }

    }

    public void CloseVote()
    {

        if (PlayerManager.Instance.IsDie) return;

        foreach (var item in profiles)
        {

            item.CloseVote();

        }

    }

    public void ShowingItem(string playerName, string items)
    {

        if (PlayerManager.Instance.IsDie) return;

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

    public void EndVote()
    {

        chattingPanel.EndVote();

    }

    [ClientRpc]
    private void OpenDiePlayerClientRPC(string userName)
    {
        endPlayerPanel.TVEffect(true);

        endPlayerPanel.GetComponentInChildren<TMP_Text>().text = $"{userName}이(가) 투표에서 죽었습니다.";
    }

    public IEnumerator OpenDiePlayer(string userName) //서버가 실행하는 코루틴
    {
        OpenDiePlayerClientRPC(userName);
        yield return new WaitForSeconds(1.5f);
        endPlayerPanel.TVEffect(false);
        yield return null;
    }

}