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
    [HideInInspector] public bool isHold = false; //일단 무엇인가 들고 있다.
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

            Instance = this;

            playerController = GetComponent<PlayerController>();
            slots = GetComponentsInChildren<SlotUI>();

            playerController.Input.OnInventoryKeyPress += HoldItemToKey;
            playerController.Input.OnDropPress += DropItemToKey;

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
        //커서&움직임 제어
        if (isShow)
        {
            Support.SettingCursorVisable(isShow);

            if (PlayerManager.Instance == null)
                playerController.Active(!isShow);
            else
                PlayerManager.Instance.localController.Active(!isShow);
        }

        //갯수 확인
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

    public bool ObtainItem(ItemDataSO data, string extraData) // 아이템 획득
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

    public void HoldItem(string itemObj, int idx, string extraData) //아이템 손에 집기
    {
        if (NowHandItem(idx) && isHold) return;

        isHold = true;

        int oldIdx = slotIdx;
        slotIdx = idx;

        SlotColor(slots[oldIdx]); //전에 집은 슬롯
        SlotColor(slots[slotIdx]); //새로 집는 슬롯

        if (slots[slotIdx].data.itemType == ItemUseType.Possible)
        {
            slotUsingText.text = $"좌클릭으로 {slots[slotIdx].data.itemName} 사용";
        }
        else
        {
            slotUsingText.text = "";
        }

        OnSlotClickEvt?.Invoke(itemObj, idx, extraData);
    }

    public void DropItem(string itemObj, int idx, string extraData) //아이템 떨구기
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

    public void Deleteltem() // 손에 든 아이템 소진
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

    private void DropItemToKey()
    {
        if (!isHold) return;

        string na = GetItemName(slotIdx);
        Debug.Log(na);
        DropItem(na, slotIdx, "");
    }

    private void HandClear()
    {
        isHold = false;
        slotUsingText.text = "";
    }

    private void SlotClear(int idx) //해당 인덱스의 아이템을 인벤토리에서 없엠 처리
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
            slot.SetColor(slot.data.itemType == ItemUseType.Possible ? orangeColor : whiteColor);
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

    public string GetItemName(int idx, ItemLanguageType languageType = ItemLanguageType.English)
    {

        if (idx == -1) return "";

        return languageType == ItemLanguageType.English ? 
            slots[idx].slotData.poolingName : slots[idx].data.itemName;

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