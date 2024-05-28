using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EnumList;
using Unity.VisualScripting;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public SlotData slotData { get; private set; }
    public int slotIndex { get; set; } //�̰� �κ��丮���� �ο�
    public bool onCursor { get; private set; } //�굵 ������ ���� ���� ������
    public ItemDataSO data {  get; private set; }

    [SerializeField] private Image itemImage;
    private Animator anim;
    private Image slotImg;

    private string extraData;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        slotImg = GetComponent<Image>();
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
        if (slotData == null) return;

        Inventory.Instance.HoldItem(slotData.poolingName, slotIndex, extraData);
    }

    public void RemoveSlot() //��Ŭ�� ��(������ ������ ������)
    {
        if (slotData == null) return;

        //Ǯ�� ����� ������ Resources���� ��������
        Inventory.Instance.DropItem(slotData.poolingName, slotIndex, extraData);
    }

    public void ResetSlot() //������ ������(���� ����)
    {
        slotData = null;
        data = null;

        itemImage.color = Color.clear;
        Inventory.Instance.PopItemText("");
    }

    public void SetColor(Color color)
    {
        slotImg.color = color;
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
        if (!onCursor) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {

            UseSlot();

        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {

            RemoveSlot();
            
        }

    }

}