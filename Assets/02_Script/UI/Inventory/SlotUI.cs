using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using WebSocketSharp;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public SlotData slotData { get; private set; }
    public int slotIndex { get; set; } //이건 인벤토리에서 부여
    public bool onCursor { get; private set; } //얘도 참조할 일이 있지 않을까
    public ItemDataSO data {  get; private set; }

    private Image slotImage;

    private void Awake()
    {
        slotImage = GetComponent<Image>();
    }

    public void InsertSlot(ItemDataSO newData)
    {
        slotData = newData.slotData;
        data = newData;
        slotImage.sprite = slotData.slotSprite;
        slotImage.color = Color.white;
    }

    public void UseSlot() //왼클릭 시(손에 들게하기)
    {
        if (!onCursor) return;
        if (slotData == null) return;

        //풀링 만들기 전까진 Resources에서 가져오자
        Inventory.Instance.HoldItem(slotData.poolingName, slotIndex);
    }

    public void RemoveSlot() //우클릭 시(아이템 밖으로 던지기)
    {
        if (!onCursor) return;
        if (slotData == null) return;

        //풀링 만들기 전까진 Resources에서 가져오자
        Inventory.Instance.DropItem(slotData.poolingName, slotIndex);
    }

    public void ResetSlot() //아이템 소진시(슬롯 비우기)
    {
        slotData = null;

        slotImage.color = Color.clear;
        Inventory.Instance.PopItemText("");
    }

    public void OnPointerEnter(PointerEventData eventData) //마우스포인터 닿으면 설명
    {
        onCursor = true;

        if (slotData == null)
        {
            Inventory.Instance.PopItemText("");
            return;
        }

        Inventory.Instance.PopItemText(slotData.slotExplanation);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onCursor = false;

        //나중에 텍스트 빼도 됨
        //Inventory.Instance.PopItemText("");
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if(eventData.button == PointerEventData.InputButton.Left)
        {

            UseSlot();

        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {

            RemoveSlot();
            
        }

    }

}