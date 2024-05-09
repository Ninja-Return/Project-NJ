using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class StoreUIController : NetworkBehaviour
{

    [SerializeField] private StorePanel panelPrefab;
    [SerializeField] private Transform storeRoot;
    [SerializeField] private StoreSystem storeSystem;
    [SerializeField] private Camera vcam;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private Transform uiTrm;

    private Dictionary<string, StorePanel> storePanels = new();

    public override void OnNetworkSpawn()
    {
        foreach (var item in storeSystem.GetStoreData())
        {

            StorePanel panel = Instantiate(panelPrefab, storeRoot);
            panel.SetUp(item, this);

            storePanels[item.data.itemName] = panel;

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
        PlayerManager.Instance.Active(true);
        PlayerManager.Instance.localController.Input.Enable();
        Support.SettingCursorVisable(false);
        vcam.gameObject.SetActive(false);   
        vcam.depth = -100;
        StartCoroutine(SetPanelCo(false));

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

    public void SetExpText(string text)
    {

        expText.text = text;

    }

    private IEnumerator SetPanelCo(bool value)
    {

        yield return new WaitForSeconds(0.7f);
        uiTrm.TVEffect(value, 0.9f);

    }

}
