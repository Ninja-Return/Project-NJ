using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EnumList;
using Unity.Netcode;
using Unity.Collections;
using DG.Tweening;

public delegate void SlotChange(string objKey, int idx, string extraData);

public class Inventory : NetworkBehaviour
{
    public static Inventory Instance { get; private set; }

    public event SlotChange OnSlotClickEvt;
    public event SlotChange OnSlotDropEvt;
    public event SlotChange OnSlotRemove;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TMP_Text slotExpText;
    [SerializeField] private TMP_Text slotUsingText;
    [SerializeField] private GameObject invenMaxText;

    private PlayerController playerController;
    private SlotUI[] slots;
    private int slotIdx;

    public bool isShow = false;
    [HideInInspector] public bool showingDelay = false;
    [HideInInspector] public bool keyPressDelay = false;
    [HideInInspector] public bool isHold = false; //ÀÏ´Ü ¹«¾ùÀÎ°¡ µé°í ÀÖ´Ù.
    [HideInInspector] public int getItemCount;

    [SerializeField] private List<ItemDataSO> firstItem = new();

    readonly Color grayColor = new Color(0.5f, 0.5f, 0.5f, 0.8f);
    readonly Color whiteColor = new Color(0.8f, 0.8f, 0.8f, 0.8f);
    readonly Color orangeColor = new Color(1f, 0.5f, 0f, 0.8f);
    readonly Color greenColor = new Color(0f, 1f, 0f, 0.8f);

    readonly float showDelay = 0.1f;
    readonly float keyDelay = 0.3f;

    private void Start()
    {

        if (IsOwner)
        {

            Instance = this; //ï¿½ï¿½ï¿½ß¿ï¿½ ï¿½ï¿½ï¿½Ì±ï¿½ï¿½ï¿½ ï¿½Ö°ï¿½ï¿½ï¿½?

            playerController = GetComponent<PlayerController>();
            slots = GetComponentsInChildren<SlotUI>();

            playerController.Input.OnInventoryKeyPress += HoldItemToKey;

            inventoryPanel.transform.localScale = Vector3.zero;

            for (int i = 0; i < slots.Length; i++) 
            {
                slots[i].slotIndex = i;
            }

            foreach (var item in firstItem)
            {

                ObtainItem(item, "");

            }

        }


    }

    private void Update()
    {
        //Ä¿¼­&¿òÁ÷ÀÓ Á¦¾î
        if (isShow)
        {
            Support.SettingCursorVisable(isShow);

            if (PlayerManager.Instance == null)
                playerController.Active(!isShow);
            else
                PlayerManager.Instance.localController.Active(!isShow);
        }

        //°¹¼ö È®ÀÎ
        if (getItemCount >= 8)
        {
            invenMaxText.SetActive(true);
        }
        else
        {
            invenMaxText.SetActive(false);
        }
    }

    #region Public

    public void SetActiveInventoryUI(bool notPlayerActiveChange = false)
    {


        isShow = !isShow;

        showingDelay = true;
        StartCoroutine(ShowDelay());

        if (!notPlayerActiveChange)
        {

            Support.SettingCursorVisable(isShow);

            if (PlayerManager.Instance == null)
                playerController.Active(!isShow);
            else
                PlayerManager.Instance.localController.Active(!isShow);

            if (isShow)
            {

                SoundManager.Play2DSound("InventoryOpen");

            }

        }

        if (isShow)
            inventoryPanel.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
        else if (!isShow)
            inventoryPanel.transform.DOScale(new Vector3(0, 0, 0), 0.2f);
    }

    public void PopItemText(string ex)
    {
        slotExpText.text = $"\"{ex}\"";
    }

    public bool ObtainItem(ItemDataSO data, string extraData) // ¾ÆÀÌÅÛ È¹µæ
    {

        for (int idx = 0; idx < slots.Length; idx++)
        {
            if (slots[idx].slotData == null)
            {
                getItemCount++;
                slots[idx].InsertSlot(data, extraData);
                SlotColor(slots[idx]);

                NetworkSoundManager.Play3DSound("GetItem", transform.position, 0.1f, 5);
                //slots[i].TouchSlot(); 
                return true;
            }
        }

        return false;
    }

    public void HoldItem(string itemObj, int idx, string extraData) //¾ÆÀÌÅÛ ¼Õ¿¡ Áý±â
    {
        if (NowHandItem(idx) && isHold) return;

        isHold = true;

        int oldIdx = slotIdx;
        slotIdx = idx;

        SlotColor(slots[oldIdx]); //Àü¿¡ ÁýÀº ½½·Ô
        SlotColor(slots[slotIdx]); //»õ·Î Áý´Â ½½·Ô

        if (slots[slotIdx].data.itemType == ItemType.Possible)
        {
            slotUsingText.text = $"ÁÂÅ¬¸¯À¸·Î {slots[slotIdx].data.itemName} »ç¿ë";
        }
        else
        {
            slotUsingText.text = "";
        }

        OnSlotClickEvt?.Invoke(itemObj, idx, extraData);
    }

    public void DropItem(string itemObj, int idx, string extraData) //¾ÆÀÌÅÛ ¶³±¸±â
    {

        NetworkSoundManager.Play3DSound("DropItem", transform.position, 0.1f, 5);

        if (NowHandItem(idx)) HandClear();
        if (extraData == null) extraData = " ";

        SlotClear(idx);

        DropItemServerRPC(itemObj, extraData);

        OnSlotDropEvt?.Invoke(itemObj, idx, extraData);
    }

    public void DropAllItem()
    {

        foreach (var item in slots)
        {

            if (item.slotData == null) continue;
            var trm = transform.root;
            ItemSpawnManager.Instance.SpawningItem(trm.position + transform.forward, item.slotData.poolingName);

        }

    }

    public void Deleteltem() // ¼Õ¿¡ µç ¾ÆÀÌÅÛ ¼ÒÁø
    {
        if (!isHold) return;

        HandClear();
        SlotClear(slotIdx);

        OnSlotRemove?.Invoke("", slotIdx, "");
    }

    #endregion

    #region Private

    private void HoldItemToKey(int value)
    {
        if (keyPressDelay) return;

        keyPressDelay = true;
        StopCoroutine(KeyDelay());
        StartCoroutine(KeyDelay());

        slots[value - 1].UseSlot();
    }

    private void HandClear()
    {
        isHold = false;
        slotUsingText.text = "";
    }

    private void SlotClear(int idx) //ÇØ´ç ÀÎµ¦½ºÀÇ ¾ÆÀÌÅÛÀ» ÀÎº¥Åä¸®¿¡¼­ ¾ø¿¥ Ã³¸®
    {
        getItemCount--;
        slots[idx].ResetSlot();
        SlotColor(slots[idx]);
    }

    private void SlotColor(SlotUI slot)
    {
        if (slot.slotData == null)
        {
            slot.SetColor(grayColor);
            return;
        }

        if (slot == slots[slotIdx] && isHold)
            slot.SetColor(greenColor);
        else
            slot.SetColor(slot.data.itemType == ItemType.Possible ? orangeColor : whiteColor);
    }

    #endregion

    #region ServerRPC

    [ServerRpc(RequireOwnership = false)]
    private void DropItemServerRPC(FixedString128Bytes itemKey, FixedString32Bytes extraData)
    {

        var item = Resources.Load<ItemRoot>($"ItemObj/{itemKey}");

        var trm = transform.root;

        var clone = Instantiate(item,
            trm.position + trm.forward,
            Quaternion.identity);

        clone.NetworkObject.Spawn();
        //clone.SetUpExtraDataServerRPC(extraData);

    }

    #endregion

    #region Return

    public bool NowHandItem(int idx)
    {
        return idx == slotIdx;
    }

    public bool GetItem(string itemName)
    {
        foreach (SlotUI slot in slots)
        {
            if (slot.data == null) continue;
            Debug.Log(slot.data.itemName);
            if (slot.data.itemName == itemName)
                return true;
        }
        return false;
    }

    public string GetItemName(int idx)
    {

        if (idx == -1) return "";

        return slots[idx].data.itemName;

    }

    #endregion

    #region Coroutine

    private IEnumerator ShowDelay()
    {
        yield return new WaitForSeconds(showDelay);
        showingDelay = false;
    }

    private IEnumerator KeyDelay()
    {

        yield return new WaitForSeconds(keyDelay);
        keyPressDelay = false;

    }

    #endregion

}