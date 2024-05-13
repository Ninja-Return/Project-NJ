using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
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
        
    }

    public void GameStart()
    {

        Debug.Log("StartGame");
        NetworkSoundManager.Play2DSound("GameStart");
        StartCoroutine(OpenText("StartAlert"));

    }

    public void HardStart()
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

    private void OnDestroy()
    {

        Instance = null;

    }

}
