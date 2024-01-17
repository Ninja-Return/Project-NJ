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
        Instance = this; //���߿� ���̱��� �ְ���?

        slots = GetComponentsInChildren<SlotUI>();
        for (int i = 0; i < slots.Length; i++) //�տ� �� ������ ���� ����� �ϴϱ�
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
                //slots[i].TouchSlot(); //���ڸ��� ���������� �̰ɷ�
                break;
            }
        }

        //���⼭�� ���� �ȸԾ����ٴ°� �˷��ټ��� �ְ�
        return;
    }

    public void HoldItem(GameObject itemObj, int idx)
    {
        isHold = true;
        slotIdx = idx;

        OnSlotClickEvt?.Invoke(itemObj); //������ ��� ���� pop�ϰ� ���ο� ������
    }

    public void Deleteltem() //������ ������ ��ǥ�� ����ͼ� ����
    {
        if (!isHold) return;
        isHold = false;

        slots[slotIdx].ResetSlot();
    }
}
