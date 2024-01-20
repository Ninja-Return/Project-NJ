using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "~~CraftData", menuName = "SO/Item/CraftData")]
public class CraftData : ScriptableObject
{
    //������ ������ ������ ItemData(������ ������)�� �ٲܰ���
    public GameObject crateItem; //SlotData�� �ٲܼ��� �ְ�
    public SlotData[] materialsItems;
}
