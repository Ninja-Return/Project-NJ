using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class HintObject : ItemRoot
{

    [SerializeField] private TMP_Text hintText;

    protected override void DoInteraction()
    {

        Debug.Log("추후 구현");

    }

    [ClientRpc]
    public void SpawnClientRPC(FixedString128Bytes hintText)
    {

        this.hintText.text = hintText.ToString();

    }

}
