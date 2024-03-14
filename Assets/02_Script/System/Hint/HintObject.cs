using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class HintObject : ItemRoot
{

    [SerializeField] private TMP_Text hintText;


    [ClientRpc]
    public void SpawnClientRPC(FixedString128Bytes hintText)
    {

        SetUpExtraData(hintText.ToString());

    }

    protected override void SetUpExtraData(string str)
    {

        base.SetUpExtraData(str);

        this.hintText.text = str;

    }

}
