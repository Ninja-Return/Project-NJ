using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumList;

public class Inventory : MonoBehaviour
{
    public event Action<GameObject> OnSlotClickEvt;

    private SlotUI[] slots;

    [HideInInspector] public bool isHold;
    private int slotIdx;

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
