using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumList;
using System.Linq;
using Unity.Netcode;
using System;

public class CraftingTable : NetworkBehaviour
{
    public event Action OnItemCraftEvt;

    private CraftData[] allCraftData;
    private List<ItemRoot> onTableItem = new();

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
            Dictionary<string, int> requiredMaterials = new Dictionary<string, int>();
            foreach (ItemDataSO itemData in craftData.materialsItems)
            {
                if (requiredMaterials.ContainsKey(itemData.slotData.poolingName))
                {
                    requiredMaterials[itemData.slotData.poolingName]++;
                }
                else
                {
                    requiredMaterials.Add(itemData.slotData.poolingName, 1);
                }
            }

            // ���̺� ���� �ִ� �����۵��� �ʿ��� ��Ḧ �����ϴ��� Ȯ��
            foreach (KeyValuePair<string, int> pair in requiredMaterials)
            {
                int count = onTableItem.Count(x => x.data.slotData.poolingName == pair.Key);
                if (count != pair.Value) //��Ȯ�� ������ �������
                {
                    isPossibleData = false;
                    break;
                }
            }

            if (isPossibleData)
            {
                //�ռ� ������ �������� ����
                NetworkObject crateItem = Instantiate(craftData.crateItem, crateItmeSpawnTrs.position ,Quaternion.identity);
                crateItem.Spawn(true);
                //�̺�Ʈ ȣ���ϰ�
                OnItemCraftEvt?.Invoke();
                //�÷��� ������ �����ϰ�
                TableItemRemoveServerRpc();
                //������
                break;
            }
        }
    }

    [ServerRpc]
    private void TableItemRemoveServerRpc()
    {
        foreach (var onTableObj in onTableItem)
            onTableObj.NetworkObject.Despawn();
    }


    private void UpdateOnTableItems()
    {
        onTableItem.Clear(); // ���� ����� ����� ���� ����

        Vector3 center = tableBoxArea.transform.position;
        Vector3 halfExtents = tableBoxArea.transform.localScale;

        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<ItemRoot>(out ItemRoot itemData))
            {
                onTableItem.Add(itemData);

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
