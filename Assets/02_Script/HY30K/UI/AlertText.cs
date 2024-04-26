using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Netcode;
using Michsky.UI.Dark;

public class AlertText : MonoBehaviour
{
    //[SerializeField] private GameObject startAlertPanel;
    public MainPanelManager mainPanelManager;
    public static AlertText Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {

        New_GameManager.Instance.OnHardEvent += HardStart;

    }

    public void GameStart()
    {

        Debug.Log("StartGame");
        NetworkSoundManager.Play2DSound("GameStart");
        StartCoroutine(OpenText("StartAlert"));

    }

    public void HardStart()
    {

        //Invoke("HardStartClientRpc", 2f);
        HardStartClientRpc();

    }

    [ClientRpc]
    private void HardStartClientRpc()
    {
        NetworkSoundManager.Play2DSound("HardStart");
        StartCoroutine(OpenText("HardAlert"));
    }

    private IEnumerator OpenText(string panelName)
    {

        mainPanelManager.OpenPanel(panelName);

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
