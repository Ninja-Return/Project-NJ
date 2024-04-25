using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public SlotData slotData { get; private set; }
    public int slotIndex { get; set; } //이건 인벤토리에서 부여
    public bool onCursor { get; private set; } //얘도 참조할 일이 있지 않을까
    public ItemDataSO data {  get; private set; }

    [Header("Slot")]
    private Animator anim;

    [SerializeField] private Image itemImage;
    private string extraData;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void InsertSlot(ItemDataSO newData, string extraData)
    {
        slotData = newData.slotData;
        data = newData;
        itemImage.sprite = slotData.slotSprite;
        itemImage.color = Color.white;
        this.extraData = extraData;
    }

    public void UseSlot() //왼클릭 시(손에 들게하기)
    {
        if (!onCursor) return;
        if (slotData == null) return;

        //풀링 만들기 전까진 Resources에서 가져오자
        Inventory.Instance.HoldItem(slotData.poolingName, slotIndex, extraData);
    }

    public void RemoveSlot() //우클릭 시(아이템 밖으로 던지기)
    {
        if (!onCursor) return;
        if (slotData == null) return;

        //풀링 만들기 전까진 Resources에서 가져오자
        Inventory.Instance.DropItem(slotData.poolingName, slotIndex, extraData);
    }

    public void ResetSlot() //아이템 소진시(슬롯 비우기)
    {
        slotData = null;

        itemImage.color = Color.clear;
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

        anim.SetBool("Hover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onCursor = false;

        anim.SetBool("Hover", false);

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