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
                Destroy(data); //이후에 pop으로 바꿔줍시다.
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
