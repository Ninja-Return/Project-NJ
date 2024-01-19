using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "~~CraftData", menuName = "SO/Item/CraftData")]
public class CraftData : ScriptableObject
{
    //준혁이 아이템 끝나면 ItemData(아이템 데이터)로 바꿀거임
    public GameObject crateItem; //SlotData로 바꿀수도 있고
    public SlotData[] materialsItems;
}
