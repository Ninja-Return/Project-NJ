using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;
using System;

public class StoreUIController : NetworkBehaviour
{

    [SerializeField] private StorePanel panelPrefab;
    [SerializeField] private Transform storeRoot;
    [SerializeField] private StoreSystem storeSystem;
    [SerializeField] private Camera vcam;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private Transform uiTrm;

    private Dictionary<string, StorePanel> storePanels = new();
    private int page;
    private int maxPage;

    public override void OnNetworkSpawn()
    {

        maxPage = storeSystem.GetStoreData().Count / 6;
        SetPanel();

    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            Exit();

        }

    }

    public void StorePanelRefresh(string itemName)
    {
        StorePanelRefreshServerRpc(itemName);
    }

    [ServerRpc(RequireOwnership = false)]
    private void StorePanelRefreshServerRpc(string itemName)
    {
        StorePanelRefreshClientRpc(itemName);
    }

    [ClientRpc]
    private void StorePanelRefreshClientRpc(string itemName)
    {
        if (storePanels.TryGetValue(itemName, out StorePanel value))
        {
            value.SetUp(storeSystem.GetStoreData(itemName).Value, this);
        }
    }

    public void Exit()
    {
        
        PlayerManager.Instance.localController.Input.Enable();
        Support.SettingCursorVisable(false);
        vcam.gameObject.SetActive(false);   
        vcam.depth = -100;
        StartCoroutine(SetPanelCo(false, () => PlayerManager.Instance.Active(true)));

    }

    public void StartSeq()
    {
        vcam.gameObject.SetActive(true);
        PlayerManager.Instance.localController.Input.Disable();
        PlayerManager.Instance.localController.playerRigidbody.velocity = Vector3.zero;
        vcam.depth = 100;
        PlayerManager.Instance.Active(false);
        Support.SettingCursorVisable(true);
        StartCoroutine(SetPanelCo(true));
    }

    public void Next()
    {

        if(!(page + 1 > maxPage))
        {


            Clear();
            page++;
            SetPanel();

        }

    }

    public void Previous()
    {

        if (!(page - 1 == -1))
        {

            Clear();
            page--;
            SetPanel();

        }

    }

    private void SetPanel()
    {

        int offset = page * 6;

        var data = storeSystem.GetStoreData();

        for (int i = 0; i < 6; i++)
        {

            if (data.Count <= i + offset) return;

            var item = data[i + offset];

            StorePanel panel = Instantiate(panelPrefab, storeRoot);
            panel.SetUp(item, this);

            storePanels[item.data.itemName] = panel;

        }


    }

    private void Clear()
    {

        int cnt = storeRoot.childCount;

        for(int i = 0; i < cnt; i++)
        {

            Destroy(storeRoot.GetChild(i).gameObject);

        }

    }

    public void SetExpText(string text)
    {

        expText.text = text;

    }

    private IEnumerator SetPanelCo(bool value, Action endCallback = null)
    {

        yield return new WaitForSeconds(0.7f);
        uiTrm.TVEffect(value, 0.9f);
        endCallback?.Invoke();

    }

}
