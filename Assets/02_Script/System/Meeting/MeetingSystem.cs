using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

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
            PhaseEnd(i + 1);

            yield return new WaitForSeconds(1);

        }

        GameManager.Instance.PlayerMoveableChangeClientRPC(true);
        DayManager.instance.TimeSetting(false);
        MeetingEndClientRPC();
        chattingSystem.ClearChatting();

    }

    private void OpenVote(ulong clientRPC, int voteCount)
    {


        OpenVoteClientRPC(voteCount);

    }

    //
    [ClientRpc]
    private void OpenVoteClientRPC(int voteCount)
    {

        meetingUI.OpenVote();

    }

}