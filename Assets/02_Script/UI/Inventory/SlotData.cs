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
    public string poolingName; //최대원식 풀링쓸때 string 필요했던거 같았는데    무튼 나중에
}
