using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Vivox;
using UnityEngine;

public class VivoxController
{

    private Channel3DProperties channel3DProperties;
    private LoginOptions loginOptions;
    private string joinCode;
    private bool isInit;
    private bool completeJoin3D, completeJoin2D;

    public VivoxController(ulong clientId, string code)
    {

        loginOptions = new LoginOptions();
        loginOptions.PlayerId = clientId.ToString();
        loginOptions.DisplayName = code + clientId.ToString();
        loginOptions.ParticipantUpdateFrequency = ParticipantPropertyUpdateFrequency.TenPerSecond;
        loginOptions.EnableTTS = false;
        joinCode = code;

        var settingData = Resources.Load<Channel3DSettingSO>("Vivox/3DSetting");
        channel3DProperties = settingData.Get3DSetting();

    }

    public async Task Init()
    {

        if (isInit) return;

        isInit = true;

        await VivoxService.Instance.LoginAsync();

    }

    public async Task JoinNormalChannel()
    {

        await Init();
        await VivoxService.Instance.JoinGroupChannelAsync(joinCode + "_Voice_Normal", ChatCapability.AudioOnly);

        completeJoin2D = true;

    }

    public async Task Join3DChannel()
    {

        await Init();
        await VivoxService.Instance.JoinPositionalChannelAsync(joinCode + "_Voice_3D", ChatCapability.AudioOnly, channel3DProperties);

        completeJoin3D = true;

    }

    public void UpdateChannelPos(GameObject speaker)
    {

        if (!completeJoin3D) return;

        VivoxService.Instance.Set3DPosition(speaker, joinCode + "_Voice_3D");

    }

    public async Task LeaveNormalChannel()
    {

        await VivoxService.Instance.LeaveChannelAsync(joinCode + "_Voice_Normal");

        completeJoin2D = false;

    }

    public async Task Leave3DChannel()
    {

        await VivoxService.Instance.LeaveChannelAsync(joinCode + "_Voice_3D");

        completeJoin3D = false;

    }

}
