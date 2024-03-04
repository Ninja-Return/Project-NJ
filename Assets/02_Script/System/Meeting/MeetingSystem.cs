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

        JoinChannel();

        DayManager.instance.TimeSetting(true);
        meetingUI.gameObject.SetActive(true);
        meetingUI.MeetingStart();

    }

    [ClientRpc]
    private void SpawnPanelClientRPC(ulong clientId, string userName)
    {

        meetingUI.SpawnPanel(clientId, userName, clientId == NetworkManager.LocalClientId);

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

        meetingUI.gameObject.SetActive(false);

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

        var list = bytes.Deserialize<VoteData>();

        meetingUI.OpenVote(list);

    }

    [ClientRpc]
    private void CloseVoteClientRPC()
    {

        meetingUI.CloseVote();

    }

    [ClientRpc]
    private void ShowItemClientRPC(FixedString32Bytes name, FixedString32Bytes item)
    {

        meetingUI.ShowingItem(name.ToString(), item.ToString());

    }

}