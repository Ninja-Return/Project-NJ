using EnumList;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "~~SlotData", menuName = "SO/Item/SlotData")]
public class SlotData : ScriptableObject
{
    public Sprite slotSprite;
    public string slotExplanation;
    public string poolingName;
}
