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
    [HideInInspector] public int slotIndex { get; set; } //�̰� �κ��丮���� �ο�

    private Inventory inventory;
    private Image slotSprite;
    private TextMeshProUGUI slotExplanation;

    public void InsertSlot(SlotData newData)
    {
        slotData = newData;
        slotSprite.sprite = slotData.slotSprite;
    }

    public void TouchSlot() //������ ��Ŭ�� ��(�տ� ����ϱ�)
    {
        if (slotData == null) return;

        //Ǯ�� ����� ������ Resources���� ��������
        GameObject itemObj = Resources.Load<GameObject>($"ItemObj/{slotData.poolingName}");
        inventory.HoldItem(itemObj, slotIndex);
    }

    public void ResetSlot() //������ ������(���� ����)
    {
        slotData = null;

        slotSprite.sprite = null;
        slotExplanation.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData) //���콺������ ������ ����
    {
        if (slotData == null) return; //�ƴ� �ؽ�Ʈ ������

        slotExplanation.text = $"\"{slotData.slotExplanation}\"";
    }
}
