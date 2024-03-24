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
    [SerializeField] private Transform selectPanel;

    private bool isVoteOpen = false;

    public static KickUIController Instance { get; private set; }

    private void Awake()
    {

        Instance = this;

    }

    public void OpenKickPanel()
    {

        selectPanel.TVEffect(true);

        for(int i = 0; i < panelRoot.childCount; i++)
        {

            Destroy(panelRoot.GetChild(i).gameObject);

        }

        foreach(var item in PlayerManager.Instance.alivePlayer)
        {

            Instantiate(kickPanel, panelRoot).SetUp(item.name.ToString(), item.clientId);

        }

    }

    public void CloseSelectPanel()
    {

        selectPanel.TVEffect(false);

    }

    private void Update()
    {

        if (!isVoteOpen) return;

        if (Input.GetKeyDown(KeyCode.Y))
        {

            KickSystem.Instance.VoteServerRPC(true);

        }
        else if (Input.GetKeyDown(KeyCode.N))
        {

            KickSystem.Instance.VoteServerRPC(false);

        }
    }

    public void OpenVotePanel(string playerName)
    {

        isVoteOpen = true;
        votePanelTrm.TVEffect(true);
        playerNameText.text = playerName;

    }

}
