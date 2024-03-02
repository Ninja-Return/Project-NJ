using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[CreateAssetMenu(fileName = "~~CraftData", menuName = "SO/Item/CraftData")]
public class CraftData : ScriptableObject, INetworkSerializeByMemcpy
{
    //������ ������ ������ ItemData(������ ������)�� �ٲܰ���
    public NetworkObject crateItem; //SlotData�� �ٲܼ��� �ְ�
    public SlotData[] materialsItems;
}
