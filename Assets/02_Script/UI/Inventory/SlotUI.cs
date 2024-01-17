using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerEnterHandler
{
    [HideInInspector] public SlotData slotData { get; private set; }
    [HideInInspector] public int slotIndex { get; set; } //이건 인벤토리에서 부여

    private Button slotBtn;
    private Image slotImage;

    private void Awake()
    {
        slotBtn = GetComponent<Button>();
        slotBtn.onClick.AddListener(TouchSlot);

        slotImage = GetComponent<Image>();
    }

    public void InsertSlot(SlotData newData)
    {
        slotData = newData;
        slotImage.sprite = slotData.slotSprite;
    }

    public void TouchSlot() //아이템 왼클릭 시(손에 들게하기)
    {
        if (slotData == null) return;

        //풀링 만들기 전까진 Resources에서 가져오자
        GameObject itemObj = Resources.Load<GameObject>($"ItemObj/{slotData.poolingName}");
        Inventory.Instance.HoldItem(itemObj, slotIndex);
    }

    public void ResetSlot() //아이템 소진시(슬롯 비우기)
    {
        slotData = null;

        slotImage.sprite = null;
        Inventory.Instance.PopItemText("");
    }

    public void OnPointerEnter(PointerEventData eventData) //마우스포인터 닿으면 설명
    {
        if (slotData == null)
        {
            Inventory.Instance.PopItemText("");
            return;
        }

        Inventory.Instance.PopItemText(slotData.slotExplanation);
    }
}
