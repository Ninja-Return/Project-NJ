using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EnumList;
using Unity.Netcode;

public delegate void SlotClick(string objKey, int idx);

public class Inventory : NetworkBehaviour
{
    public static Inventory Instance;

    public event SlotClick OnSlotClickEvt; //��ȣ�ۿ뿡�� ������ �տ���� �Լ� �־���� �����
    public event Action<string> OnSlotDropEvt; //��� ������ �����鼭 ������ �Լ� �־���

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

            Instance = this; //���߿� ���̱��� �ְ���?

            slots = GetComponentsInChildren<SlotUI>();
            for (int i = 0; i < slots.Length; i++) //�տ� �� ������ ���� ����� �ϴϱ�
            {
                slots[i].slotIndex = i;
            }

            inventoryPanel.SetActive(false);

        }


    }

    public void SetActiveInventoryUI() //�÷��̾� ��ǲ�� ����
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

    public void ObtainItem(SlotData data) //������ ������ �ҷ��� ��ǥ��
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

    public void HoldItem(string itemObj, int idx)
    {
        isHold = true;
        slotIdx = idx;

        OnSlotClickEvt?.Invoke(itemObj, idx); //������ ��� ���� pop�ϰ� ���ο� ������
    }

    public void DropItem(string itemObj, int idx)
    {
        slotIdx = idx;
        slots[slotIdx].ResetSlot();

        OnSlotDropEvt?.Invoke(itemObj); //�տ��� ������ ��ô
    }

    public void Deleteltem() //��ȸ�� ������ ������ ��ǥ�� ����ͼ� ����
    {
        if (!isHold) return;
        isHold = false;

        slots[slotIdx].ResetSlot();
    }
}