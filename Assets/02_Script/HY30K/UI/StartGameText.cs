using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Netcode;
using Michsky.UI.Dark;

public class StartGameText : MonoBehaviour
{
    //[SerializeField] private GameObject startAlertPanel;
    public MainPanelManager mainPanelManager;
    public static StartGameText Instance;

    private void Awake()
    {
        Instance = this;
    }

    /*public void DOStart()
    {

        New_GameManager.Instance.OnGameStarted += HandleGameStartedClientRpc;

    }*/

    public void GameStart()
    {

        Debug.Log("StartGame");
        NetworkSoundManager.Play2DSound("GameStart");
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
