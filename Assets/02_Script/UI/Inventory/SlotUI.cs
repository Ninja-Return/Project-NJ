using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerEnterHandler
{
    [HideInInspector] public SlotData slotData { get; private set; }
    [HideInInspector] public int slotIndex { get; set; } //�̰� �κ��丮���� �ο�

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

    public void TouchSlot() //������ ��Ŭ�� ��(�տ� ����ϱ�)
    {
        if (slotData == null) return;

        //Ǯ�� ����� ������ Resources���� ��������
        GameObject itemObj = Resources.Load<GameObject>($"ItemObj/{slotData.poolingName}");
        Inventory.Instance.HoldItem(itemObj, slotIndex);
    }

    public void ResetSlot() //������ ������(���� ����)
    {
        slotData = null;

        slotImage.sprite = null;
        Inventory.Instance.PopItemText("");
    }

    public void OnPointerEnter(PointerEventData eventData) //���콺������ ������ ����
    {
        if (slotData == null)
        {
            Inventory.Instance.PopItemText("");
            return;
        }

        Inventory.Instance.PopItemText(slotData.slotExplanation);
    }
}
