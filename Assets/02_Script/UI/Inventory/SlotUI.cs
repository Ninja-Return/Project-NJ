using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public SlotData slotData { get; private set; }
    [HideInInspector] public int slotIndex { get; set; } //�̰� �κ��丮���� �ο�
    [HideInInspector] public bool onCursor { get; private set; } //�굵 ������ ���� ���� ������

    private Image slotImage;

    private void Awake()
    {
        slotImage = GetComponent<Image>();
    }

    public void InsertSlot(SlotData newData)
    {
        slotData = newData;
        slotImage.sprite = slotData.slotSprite;
    }

    public void UseSlot() //��Ŭ�� ��(�տ� ����ϱ�)
    {
        if (slotData == null) return;

        //Ǯ�� ����� ������ Resources���� ��������
        GameObject itemObj = Resources.Load<GameObject>($"ItemObj/{slotData.poolingName}");
        Inventory.Instance.HoldItem(itemObj, slotIndex);
    }

    public void RemoveSlot() //��Ŭ�� ��(������ ������ ������)
    {
        if (slotData == null) return;

        //Ǯ�� ����� ������ Resources���� ��������
        GameObject itemObj = Resources.Load<GameObject>($"ItemObj/{slotData.poolingName}");
        Inventory.Instance.DropItem(itemObj, slotIndex);
    }

    public void ResetSlot() //������ ������(���� ����)
    {
        slotData = null;

        slotImage.sprite = null;
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
}
