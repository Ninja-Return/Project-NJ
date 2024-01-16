using EnumList;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "~~SlotData", menuName = "Item/SlotData")]
public class SlotData : ScriptableObject
{
    public ItemType slotType;
    public Sprite slotSprite;
    public string slotExplanation;
    public string poolingName; //�ִ���� Ǯ������ string �ʿ��ߴ��� ���Ҵµ�    ��ư ���߿�
}
