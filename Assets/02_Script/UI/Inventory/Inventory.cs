using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EnumList;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public event Action<GameObject> OnSlotClickEvt;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TextMeshProUGUI slotExpText;
    private SlotUI[] slots;
    private int slotIdx;

    [HideInInspector] public bool isShow = false;
    [HideInInspector] public bool isHold = false;

    private void Awake()
    {
        Instance = this; //나중에 모노싱글톤 있겠지?

        slots = GetComponentsInChildren<SlotUI>();
        for (int i = 0; i < slots.Length; i++) //손에 든 아이템 쓸때 비워야 하니까
        {
            slots[i].slotIndex = i;
        }
    }

    private void SetActiveInventoryUI()
    {
        isShow = !isShow;
        inventoryPanel.SetActive(isShow);
    }

    public void PopItemText(string ex)
    {
        slotExpText.text = $"\"{ex}\"";
    }

    public void ObtainItem(SlotData data)
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

    public void HoldItem(GameObject itemObj, int idx)
    {
        isHold = true;
        slotIdx = idx;

        OnSlotClickEvt?.Invoke(itemObj); //기존에 들던 무기 pop하고 새로운 무기들기
    }

    public void Deleteltem() //아이템 소진시 준표가 갇고와서 발행
    {
        if (!isHold) return;
        isHold = false;

        slots[slotIdx].ResetSlot();
    }
}
