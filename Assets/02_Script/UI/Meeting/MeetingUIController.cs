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

    public void MeetingStart()
    {
        
        for(int i = 0; i < panelRoot.childCount; i++)
        {

            Destroy(panelRoot.GetChild(i).gameObject);

        }

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

    }

    public void ChattingOpen()
    {

        bool active = !chattingPanel.gameObject.activeSelf;

        chattingPanel.Init(active);
        chattingPanel.gameObject.SetActive(active);

    }

}
