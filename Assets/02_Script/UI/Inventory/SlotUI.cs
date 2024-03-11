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
    public int slotIndex { get; set; } //�̰� �κ��丮���� �ο�
    public bool onCursor { get; private set; } //�굵 ������ ���� ���� ������
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

    public void UseSlot() //��Ŭ�� ��(�տ� ����ϱ�)
    {
        if (!onCursor) return;
        if (slotData == null) return;

        //Ǯ�� ����� ������ Resources���� ��������
        Inventory.Instance.HoldItem(slotData.poolingName, slotIndex);
    }

    public void RemoveSlot() //��Ŭ�� ��(������ ������ ������)
    {
        if (!onCursor) return;
        if (slotData == null) return;

        //Ǯ�� ����� ������ Resources���� ��������
        Inventory.Instance.DropItem(slotData.poolingName, slotIndex);
    }

    public void ResetSlot() //������ ������(���� ����)
    {
        slotData = null;

        slotImage.color = Color.clear;
        Inventory.Instance.PopItemText("");
    }

    public void OnPointerEnter(PointerEventData eventData) //���콺������ ������ ����
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

        //���߿� �ؽ�Ʈ ���� ��
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