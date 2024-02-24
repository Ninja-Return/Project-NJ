using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class MeetingSystem : NetworkBehaviour
{

    [SerializeField] private GameObject mettingUI;
    [SerializeField] private Transform panelRoot;
    [SerializeField] private MeetingPanel panelPrefab;
    [SerializeField] private TMP_Text phaseText;
    [SerializeField] private TMP_Text phaseCountText;

    private NetworkVariable<FixedString32Bytes> phaseTextBase = new();
    private NetworkVariable<int> phaseCountBase = new();
    private Dictionary<ulong, int> voteContainer = new();
    private bool isVote = false;

    private readonly int phaseTime = 5;

    public static MeetingSystem Instance { get; private set; }

    private void Awake()
    {

        Instance = this;

    }

    private void Start()
    {

        phaseTextBase.OnValueChanged += HandleTextChanged;
        phaseCountBase.OnValueChanged += HandleCountChanged;

        if (!IsServer) return;

        DayManager.instance.OnDayComming += HandleMettingOpen;

    }

    private void HandleCountChanged(int previousValue, int newValue)
    {

        phaseCountText.text = newValue == 0 ? "" : newValue.ToString();

    }

    private void HandleTextChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {

        phaseText.text = newValue.ToString();

    }

    private void HandleMettingOpen()
    {

        GameManager.Instance.PlayerMoveableChangeClientRPC(false);

        MettingOpenClientRPC();

        foreach(var item in NetworkManager.ConnectedClientsIds)
        {

            var data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(item);

            if(data != null)
            {

                SpawnPanelClientRPC(item, data.Value.nickName);

            }

        }

        StartCoroutine(MeetingCountingCo());

    }

    [ClientRpc]
    private void MettingOpenClientRPC()
    {

        JoinChannel();

        DayManager.instance.TimeSetting(true);
        mettingUI.SetActive(true);

    }

    [ClientRpc]
    private void SpawnPanelClientRPC(ulong clientId, string userName)
    {

        Instantiate(panelPrefab, panelRoot).Setting(clientId, userName, clientId == NetworkManager.LocalClientId);

    }

    private async void JoinChannel()
    {

        await NetworkController.Instance.vivox.Leave3DChannel();
        await NetworkController.Instance.vivox.JoinNormalChannel();

    }

    private async void Join3DChannel()
    {

        await NetworkController.Instance.vivox.LeaveNormalChannel();
        await NetworkController.Instance.vivox.Join3DChannel();

    }

    [ClientRpc]
    private void MeetingEndClientRPC()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Join3DChannel();

        mettingUI.gameObject.SetActive(false);

    }

    [ClientRpc]
    private void PhaseEndClientRPC()
    {

        isVote = false;

    }

    private void PhaseEnd(int phase)
    {

        int maxVoteCount = int.MinValue;
        List<ulong> maxVoteClient = new();

        foreach(var item in voteContainer)
        {

            if(item.Value > maxVoteCount)
            {

                maxVoteClient.Clear();
                maxVoteClient.Add(item.Key);

                maxVoteCount = item.Value;

            }
            else if(item.Value == maxVoteCount)
            {

                maxVoteClient.Add(item.Key);

            }

        }

        if (maxVoteClient.Count > 1)
        {

            Debug.Log("���ƾ� �ù߾�");

        }
        else if (maxVoteClient.Count == 1)
        {

            Debug.Log(maxVoteClient[0]);

            if (phase == 1)
            {

                var data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(maxVoteClient[0]);

                foreach(var item in data.Value.attachedItem)
                {

                    Debug.Log(item);

                }

            }
            else if (phase == 2)
            {

                GameManager.Instance.PlayerDie(maxVoteClient[0]);

            }

        }


        PhaseEndClientRPC();

    }

    [ServerRpc(RequireOwnership = false)]
    private void VoteServerRPC(ulong clientId)
    {

        if(!voteContainer.ContainsKey(clientId)) 
        {

            voteContainer.Add(clientId, 0);

        }

        voteContainer[clientId]++;

    }

    public void Vote(ulong clientId)
    {

        if (clientId == NetworkManager.LocalClientId || isVote) return;

        isVote = true;

        VoteServerRPC(clientId);

    }

    private IEnumerator MeetingCountingCo()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        for(int i = 0; i < 3; i++)
        {

            phaseTextBase.Value = $"Phase{i + 1}";

            for(int j = phaseTime; j > 0; j--)
            {

                phaseCountBase.Value = j;
                yield return new WaitForSeconds(1);

            }

            phaseCountBase.Value = 0;
            PhaseEnd(i + 1);

            yield return new WaitForSeconds(1);

        }

        GameManager.Instance.PlayerMoveableChangeClientRPC(true);
        DayManager.instance.TimeSetting(false);
        MeetingEndClientRPC();

    }

}