using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MeetingSystem : NetworkBehaviour
{

    private void Start()
    {

        if (!IsServer) return;

        DayManager.instance.OnDayComming += HandleMettingOpen;
        DayManager.instance.OnNightComming += HandleMettingOpen;

    }

    private void HandleMettingOpen()
    {



    }

}
