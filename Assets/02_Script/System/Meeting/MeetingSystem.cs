using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public struct VoteData
{

    public ulong clientId;
    public int voteCount;

}

public class MeetingSystem : NetworkBehaviour
{

    [SerializeField] private MeetingUIController meetingUI;
    [SerializeField] private ChattingSystem chattingSystem;

    private NetworkVariable<int> phaseCountBase = new();
    private NetworkVariable<int> phaseTimeBase = new();
    private Dictionary<ulong, int> voteContainer = new();
    private bool isVote = false;

    private readonly int phaseTime = 3;

    public static MeetingSystem Instance { get; private set; }

    public event Action OnMeetingEnd;

    private void Awake()
    {

        Instance = this;

    }

    private void Start()
    {

        phaseCountBase.OnValueChanged += HandlePhaseChanged;
        phaseTimeBase.OnValueChanged += HandleTimeChanged;

        if (!IsServer) return;

        DayManager.instance.OnDayComming += HandleMettingOpen;

    }

    private void HandleTimeChanged(int previousValue, int newValue)
    {

        meetingUI.UpdateTime(newValue);

    }

    private void HandlePhaseChanged(int previousValue, int newValue)
    {

        meetingUI.PhaseChange(newValue);

    }

    private void HandleMettingOpen()
    {

        GameManager.Instance.PlayerMoveableChangeClientRPC(false);

        chattingSystem.ClearChatting();

        MettingOpenClientRPC();

        foreach(var item in NetworkManager.ConnectedClientsIds)
        {

            var data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(item);

            if(data != null && data.Value.isDie == false)
            {

                SpawnPanelClientRPC(item, data.Value.nickName);

            }

        }

        StartCoroutine(MeetingCountingCo());

    }

    [ClientRpc]
    private void MettingOpenClientRPC()
    {

        if (GameManager.Instance.isDie) return;

        //JoinChannel();

        DayManager.instance.TimeSetting(true);
        meetingUI.gameObject.SetActive(true);
        meetingUI.MeetingStart();

    }

    [ClientRpc]
    private void SpawnPanelClientRPC(ulong clientId, string userName)
    {

        if (GameManager.Instance.isDie) return;

        meetingUI.SpawnPanel(clientId, userName, clientId == NetworkManager.LocalClientId);

    }


    [ClientRpc]
    private void MeetingEndClientRPC()
    {

        meetingUI.gameObject.SetActive(false);

        if (GameManager.Instance.isDie) return;
        
        GameManager.Instance.SettingCursorVisable(false);

        //Join3DChannel();
        //NetworkController.Instance.vivox.LeaveNormalChannel();


    }

    [ClientRpc]
    private void PhaseEndClientRPC()
    {

        if (GameManager.Instance.isDie) return;

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

            Debug.Log("°ãÃÆ¾î ½Ã¹ß¾Æ");

        }
        else if (maxVoteClient.Count == 1)
        {

            Debug.Log(maxVoteClient[0]);

            if (phase == 1)
            {

                var data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(maxVoteClient[0]).Value;

                ShowItemClientRPC(data.nickName, string.Join(',', data.attachedItem));

            }
            else if (phase == 2)
            {

                GameManager.Instance.PlayerDie(maxVoteClient[0]);

            }

        }

        voteContainer.Clear();

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

    public bool Vote(ulong clientId)
    {

        if (clientId == NetworkManager.LocalClientId || isVote) return false;

        isVote = true;

        VoteServerRPC(clientId);

        return true;

    }

    private IEnumerator MeetingCountingCo()
    {

        for(int i = 0; i < 2; i++)
        {

            phaseCountBase.Value = i;

            for(int j = phaseTime; j > 0; j--)
            {

                phaseTimeBase.Value = j;
                yield return new WaitForSeconds(1);

            }

            phaseTimeBase.Value = 0;

            OpenVote();

            yield return new WaitForSeconds(5);

            PhaseEnd(i + 1);
            CloseVoteClientRPC();

            yield return new WaitForSeconds(1);

        }

        GameManager.Instance.PlayerMoveableChangeClientRPC(true);
        DayManager.instance.TimeSetting(false);
        MeetingEndClientRPC();
        chattingSystem.ClearChatting();

        yield return null;

        OnMeetingEnd?.Invoke();

    }

    private void OpenVote()
    {

        RPCList<VoteData> voteList = new RPCList<VoteData>();

        foreach(var item in voteContainer)
        {

            voteList.list.Add(new VoteData { clientId = item.Key, voteCount = item.Value });

        }

        VoteOpenClientRPC(voteList.Serialize());

    }

    [ClientRpc]
    private void VoteOpenClientRPC(byte[] bytes)
    {

        if (GameManager.Instance.isDie) return;

        var list = bytes.Deserialize<VoteData>();

        meetingUI.OpenVote(list);

    }

    [ClientRpc]
    private void CloseVoteClientRPC()
    {

        if (GameManager.Instance.isDie) return;

        meetingUI.CloseVote();

    }

    [ClientRpc]
    private void ShowItemClientRPC(FixedString128Bytes name, FixedString32Bytes item)
    {

        if (GameManager.Instance.isDie) return;

        meetingUI.ShowingItem(name.ToString(), item.ToString());

    }

}