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

    public void ObtainItem(SlotData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].slotData == null)
            {
                slots[i].InsertSlot(data);
                Destroy(data); //���Ŀ� pop���� �ٲ��ݽô�.
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
