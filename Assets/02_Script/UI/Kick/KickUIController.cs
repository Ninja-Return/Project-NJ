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

        if (PlayerManager.Instance.IsDie) return;

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
            CloseVotePanel();

        }
        else if (Input.GetKeyDown(KeyCode.N))
        {

            KickSystem.Instance.VoteServerRPC(false);
            CloseVotePanel();

        }
    }

    public void OpenVotePanel(string playerName)
    {

        if (PlayerManager.Instance.IsDie) return;

        isVoteOpen = true;
        votePanelTrm.TVEffect(true);
        playerNameText.text = playerName;

    }

    public void CloseVotePanel()
    {

        PlayerManager.Instance.Active(true);
        isVoteOpen = false;
        votePanelTrm.TVEffect(false);

    }

}
