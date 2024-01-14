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
    private bool completeJoin;

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

    public async void JoinNormalChannel()
    {

        await Init();
        await VivoxService.Instance.JoinGroupChannelAsync(joinCode + "_Voice_Normal", ChatCapability.AudioOnly);

        completeJoin = true;

    }

    public async void Join3DChannel()
    {

        await Init();
        await VivoxService.Instance.JoinPositionalChannelAsync(joinCode + "_Voice_3D", ChatCapability.AudioOnly, channel3DProperties);

        completeJoin = true;

    }

    public void UpdateChannelPos(GameObject speaker)
    {

        if (!completeJoin) return;

        VivoxService.Instance.Set3DPosition(speaker, joinCode + "_Voice_3D");

    }

}
