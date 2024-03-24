using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreUIController : MonoBehaviour
{

    [SerializeField] private StorePanel panelPrefab;
    [SerializeField] private Transform storeRoot;
    [SerializeField] private StoreSystem storeSystem;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private Transform uiTrm;

    private void Awake()
    {
        
        foreach(var item in storeSystem.storeList)
        {

            Instantiate(panelPrefab, storeRoot).SetUp(item, this);

        }

    }

    public void Exit()
    {

        PlayerManager.Instance.Active(true);
        Support.SettingCursorVisable(false);
        vcam.Priority = -100;
        StartCoroutine(SetPanelCo(false));

    }

    public void StartSeq()
    {

        vcam.Priority = 100;
        PlayerManager.Instance.Active(false);
        Support.SettingCursorVisable(true);
        StartCoroutine(SetPanelCo(true));

    }

    public void SetExpText(string text)
    {

        expText.text = text;

    }

    private IEnumerator SetPanelCo(bool value)
    {

        yield return new WaitForSeconds(0.7f);
        uiTrm.TVEffect(value);

    }

}
