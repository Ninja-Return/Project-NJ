using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerEnterHandler
{
    [HideInInspector] public SlotData slotData { get; private set; }
    [HideInInspector] public int slotIndex { get; set; } //이건 인벤토리에서 부여

    private Inventory inventory;
    private Image slotSprite;
    private TextMeshProUGUI slotExplanation;

    public void InsertSlot(SlotData newData)
    {
        slotData = newData;
        slotSprite.sprite = slotData.slotSprite;
    }

    public void TouchSlot() //아이템 왼클릭 시(손에 들게하기)
    {
        if (slotData == null) return;

        //풀링 만들기 전까진 Resources에서 가져오자
        GameObject itemObj = Resources.Load<GameObject>($"ItemObj/{slotData.poolingName}");
        inventory.HoldItem(itemObj, slotIndex);
    }

    public void ResetSlot() //아이템 소진시(슬롯 비우기)
    {
        slotData = null;

        slotSprite.sprite = null;
        slotExplanation.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData) //마우스포인터 닿으면 설명
    {
        if (slotData == null) return; //아님 텍스트 비우던지

        slotExplanation.text = $"\"{slotData.slotExplanation}\"";
    }
}
