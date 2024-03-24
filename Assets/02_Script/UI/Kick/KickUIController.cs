using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KickUIController : MonoBehaviour
{

    [SerializeField] private Transform panelRoot;
    [SerializeField] private KickPanel kickPanel;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private Transform votePanelTrm;

    private bool isVoteOpen = false;

    public static KickUIController Instance { get; private set; }

    private void Awake()
    {

        Instance = this;

    }

    public void OpenKickPanel()
    {

        for(int i = 0; i < panelRoot.childCount; i++)
        {

            Destroy(panelRoot.GetChild(i).gameObject);

        }

        foreach(var item in PlayerManager.Instance.alivePlayer)
        {

            Instantiate(kickPanel, panelRoot).SetUp(item.name.ToString(), item.clientId);

        }

    }

    public void OpenVotePanel(string playerName)
    {

        isVoteOpen = true;
        votePanelTrm.TVEffect(true);
        playerNameText.text = playerName;

    }

}
