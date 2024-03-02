using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumList;
using System.Linq;
using Unity.Netcode;

public class CraftingTable : NetworkBehaviour
{
    private CraftData[] allCraftData;
    private List<SlotData> onTableItem; //�ϵ� ItemData�� �ٲܰ���

    [SerializeField] private GameObject tableBoxArea;
    [SerializeField] private Transform crateItmeSpawnTrs;

    private void Awake()
    {
        allCraftData = Resources.LoadAll<CraftData>("CraftData");
    }

    public void CraftingItem() //��ư ������ ȣ��
    {
        if (!IsServer) return;

        //craftingCol�� �پ��ִ� �����۵� onTableItem�� �ֱ�
        UpdateOnTableItems();

        foreach (CraftData craftData in allCraftData)
        {
            bool isPossibleData = true;

            // �� ����� �ʿ��� ������ ������ Dictionary ����
            Dictionary<ItemType, int> requiredMaterials = new Dictionary<ItemType, int>();
            foreach (SlotData slotData in craftData.materialsItems)
            {
                if (requiredMaterials.ContainsKey(slotData.slotType))
                {
                    requiredMaterials[slotData.slotType]++;
                }
                else
                {
                    requiredMaterials.Add(slotData.slotType, 1);
                }
            }

            // ���̺� ���� �ִ� �����۵��� �ʿ��� ��Ḧ �����ϴ��� Ȯ��
            foreach (KeyValuePair<ItemType, int> pair in requiredMaterials)
            {
                int count = onTableItem.Count(x => x.slotType == pair.Key);
                if (count != pair.Value) //��Ȯ�� ������ �������
                {
                    isPossibleData = false;
                    break;
                }
            }

            if (isPossibleData)
            {
                CreateItemServerRpc(craftData);
                //������
                break;
            }
        }
    }

    [ServerRpc]
    private void CreateItemServerRpc(CraftData craftData)
    {
        GameObject crateItem = Instantiate(craftData.crateItem, crateItmeSpawnTrs);
        RemoveItemsFromTable();

        CreateItemClientRpc(craftData);
    }

    [ClientRpc]
    private void CreateItemClientRpc(CraftData craftData)
    {
        GameObject crateItem = Instantiate(craftData.crateItem, crateItmeSpawnTrs);
        RemoveItemsFromTable();
    }

    private void RemoveItemsFromTable()
    {
        foreach (SlotData onTableObj in onTableItem)
        {
            Destroy(onTableObj);
        }
    }

    private void UpdateOnTableItems()
    {
        onTableItem.Clear(); // ���� ����� ����� ���� ����

        Vector3 center = tableBoxArea.transform.position;
        Vector3 halfExtents = tableBoxArea.transform.localScale;

        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<SlotData>(out SlotData slotData))
            {
                onTableItem.Add(slotData);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 center = tableBoxArea.transform.position;
        Vector3 halfExtents = tableBoxArea.transform.localScale;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, halfExtents);
    }
#endif
}
