using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EnumList;
using Unity.Netcode;
using Unity.Collections;

public delegate void SlotChange(string objKey, int idx);

public class Inventory : NetworkBehaviour
{
    public static Inventory Instance { get; private set; }

    public event SlotChange OnSlotClickEvt; //상호작용에서 아이템 손에드는 함수 넣어줘라 웅언아
    public event SlotChange OnSlotDropEvt; //얘는 아이템 던지면서 버릴때 함수 넣어줘

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TextMeshProUGUI slotExpText;
    private SlotUI[] slots;
    private int slotIdx;

    [HideInInspector] public bool isShow = false;
    [HideInInspector] public bool isHold = false;

    private void Start()
    {

        if (IsOwner)
        {

            Instance = this; //나중에 모노싱글톤 있겠지?

            slots = GetComponentsInChildren<SlotUI>();
            for (int i = 0; i < slots.Length; i++) //손에 든 아이템 쓸때 비워야 하니까
            {
                slots[i].slotIndex = i;
            }

            inventoryPanel.SetActive(false);

        }


    }

    public void SetActiveInventoryUI() //플레이어 인풋과 연결
    {
        isShow = !isShow;

        Cursor.visible = isShow;
        Cursor.lockState = isShow ? CursorLockMode.None : CursorLockMode.Locked;

        inventoryPanel.SetActive(isShow);
    }

    public void PopItemText(string ex)
    {
        slotExpText.text = $"\"{ex}\"";
    }

    public void ObtainItem(ItemDataSO data) //아이템 먹을때 불러줘 준표씨
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].slotData == null)
            {
                slots[i].InsertSlot(data);
                //slots[i].TouchSlot(); //먹자마자 들고있을라면 이걸로
                break;
            }
        }

        //여기서는 뭔가 안먹어진다는걸 알려줄수도 있고
        return;
    }

    public void HoldItem(string itemObj, int idx)
    {
        isHold = true;
        slotIdx = idx;

        OnSlotClickEvt?.Invoke(itemObj, idx); //기존에 들던 무기 pop하고 새로운 무기들기
    }

    public void DropItem(string itemObj, int idx)
    {
        slotIdx = idx;
        slots[slotIdx].ResetSlot();

        DropItemServerRPC(itemObj);

        OnSlotDropEvt?.Invoke(itemObj, idx); //손에서 아이템 투척
    }

    public void Deleteltem() //일회용 아이템 소진시 준표가 갇고와서 발행
    {
        if (!isHold) return;
        isHold = false;

        slots[slotIdx].ResetSlot();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DropItemServerRPC(FixedString128Bytes itemKey)
    {

        var item = Resources.Load<ItemRoot>($"ItemObj/{itemKey}");

        var trm = transform.root;

        var clone = Instantiate(item,
            trm.position + trm.forward, 
            Quaternion.identity);

        clone.NetworkObject.Spawn();

    }

    public string GetItemName(int idx)
    {

        if (idx == -1) return "";

        return slots[idx].data.itemName;

    }

}