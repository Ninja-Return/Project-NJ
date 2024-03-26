using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Netcode;

public class StartGameText : NetworkBehaviour
{
    [SerializeField] private GameObject startAlertPanel;

    private void Start()
    {

        if (IsServer)
        {

            New_GameManager.Instance.OnGameStarted += HandleGameStarted;

        }

    }

    private void HandleGameStarted()
    {

        StartCoroutine(StartAlert());

    }

    private IEnumerator StartAlert()
    {

        startAlertPanel.transform.TVEffect(true);

        yield return new WaitForSeconds(3f);

        startAlertPanel.transform.TVEffect(false);

    }
}
