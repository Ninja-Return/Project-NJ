using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Netcode;
using Michsky.UI.Dark;

public class StartGameText : NetworkBehaviour
{
    //[SerializeField] private GameObject startAlertPanel;
    public UIDissolveEffect dissolveEffect;
    public MainPanelManager mainPanelManager;

    private void Start()
    {

        return;

        //¿Ã∞≈ ≥Œ∂‰
        dissolveEffect.DissolveOut();

    }

    public void DOStart()
    {
        if (IsServer)
        {

            New_GameManager.Instance.OnGameStarted += HandleGameStartedClientRpc;

        }
    }

    [ClientRpc]
    private void HandleGameStartedClientRpc()
    {

        NetworkSoundManager.Play2DSound("GameStart");
        WaitRoomManager.Instance.UnActiveLoadingPanel();
        StartCoroutine(StartText());

    }

    private IEnumerator StartText()
    {

        mainPanelManager.OpenPanel("Alert");

        yield return new WaitForSeconds(3f);

        mainPanelManager.OpenFirstTab();

    }

    /* private IEnumerator StartAlert()
     {

         startAlertPanel.transform.TVEffect(true);

         yield return new WaitForSeconds(3f);

         startAlertPanel.transform.TVEffect(false);

     }*/
}
