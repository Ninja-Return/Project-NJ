using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumList;
using System.Linq;
using Unity.Netcode;

public class CraftingTable : NetworkBehaviour
{
    private CraftData[] allCraftData;
    private List<SlotData> onTableItem; //니도 ItemData로 바꿀거임

    [SerializeField] private GameObject tableBoxArea;
    [SerializeField] private Transform crateItmeSpawnTrs;

    private void Awake()
    {
        allCraftData = Resources.LoadAll<CraftData>("CraftData");
    }

    public void CraftingItem() //버튼 누를때 호출
    {
        if (!IsServer) return;

        //craftingCol에 붙어있는 아이템들 onTableItem에 넣기
        UpdateOnTableItems();

        foreach (CraftData craftData in allCraftData)
        {
            bool isPossibleData = true;

            // 각 재료의 필요한 개수를 저장할 Dictionary 생성
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

            // 테이블 위에 있는 아이템들이 필요한 재료를 충족하는지 확인
            foreach (KeyValuePair<ItemType, int> pair in requiredMaterials)
            {
                int count = onTableItem.Count(x => x.slotType == pair.Key);
                if (count != pair.Value) //정확한 개수를 맟춰야지
                {
                    isPossibleData = false;
                    break;
                }
            }

            if (isPossibleData)
            {
                CreateItemServerRpc(craftData);
                //나가기
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
        onTableItem.Clear(); // 기존 목록을 지우고 새로 시작

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
