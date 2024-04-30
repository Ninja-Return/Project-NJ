using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public SlotData slotData { get; private set; }
    public int slotIndex { get; set; } //�̰� �κ��丮���� �ο�
    public bool onCursor { get; private set; } //�굵 ������ ���� ���� ������
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

    public void UseSlot() //��Ŭ�� ��(�տ� ����ϱ�)
    {
        if (!onCursor) return;
        if (slotData == null) return;

        //Ǯ�� ����� ������ Resources���� ��������
        Inventory.Instance.HoldItem(slotData.poolingName, slotIndex, extraData);
    }

    public void RemoveSlot() //��Ŭ�� ��(������ ������ ������)
    {
        if (!onCursor) return;
        if (slotData == null) return;

        //Ǯ�� ����� ������ Resources���� ��������
        Inventory.Instance.DropItem(slotData.poolingName, slotIndex, extraData);
    }

    public void ResetSlot() //������ ������(���� ����)
    {
        slotData = null;

        itemImage.color = Color.clear;
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

        anim.SetBool("Hover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onCursor = false;

        anim.SetBool("Hover", false);

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