using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;
using FSM_System.Netcode;

public class PlayerStateRoot : FSM_State_Netcode<EnumPlayerState>
{

    protected Rigidbody rigid;
    protected PlayerDataSO data;

    public PlayerStateRoot(PlayerController controller) : base(controller)
    {

        rigid = GetComponent<Rigidbody>();
        data = controller.Data;

    }


}