using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class KickSystem : NetworkBehaviour
{
    
    public static KickSystem Instance { get; private set; }

    private ulong selectClientId;
    private int allCnt;

    private void Awake()
    {
        
        Instance = this;

    }

    public void SelectPlayer(ulong clientId)
    {

        SelectPlayerServerRPC(clientId);

    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectPlayerServerRPC(ulong clientId)
    {

        var data = HostSingle.Instance.NetServer.GetUserDataByClientID(clientId).Value;
        selectClientId = clientId;

        SelectPlayerClientRPC(data.nickName);

        StartCoroutine(WaitVoteCo());

    }

    [ClientRpc]
    private void SelectPlayerClientRPC(FixedString64Bytes name)
    {

        KickUIController.Instance.OpenVotePanel(name.ToString());

    }

    [ServerRpc(RequireOwnership = false)]
    public void VoteServerRPC(bool isAll)
    {

        if (isAll)
        {

            allCnt++;

        }

    }

    private IEnumerator WaitVoteCo()
    {

        yield return new WaitForSeconds(10f);

        if(allCnt >= PlayerManager.Instance.alivePlayer.Count / 2)
        {

            PlayerManager.Instance.PlayerDie(EnumList.DeadType.Vote, selectClientId);

        }

        allCnt = 0;
        selectClientId = 0;

    }

}
