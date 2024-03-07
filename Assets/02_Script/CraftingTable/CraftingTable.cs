using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumList;
using System.Linq;
using Unity.Netcode;

public class CraftingTable : NetworkBehaviour
{
    private CraftData[] allCraftData;
    private List<ItemRoot> onTableItem = new();

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
            foreach (ItemDataSO itemData in craftData.materialsItems)
            {
                if (requiredMaterials.ContainsKey(itemData.slotData.slotType))
                {
                    requiredMaterials[itemData.slotData.slotType]++;
                }
                else
                {
                    requiredMaterials.Add(itemData.slotData.slotType, 1);
                }
            }

            // 테이블 위에 있는 아이템들이 필요한 재료를 충족하는지 확인
            foreach (KeyValuePair<ItemType, int> pair in requiredMaterials)
            {
                int count = onTableItem.Count(x => x.data.slotData.slotType == pair.Key);
                if (count != pair.Value) //정확한 개수를 맟춰야지
                {
                    isPossibleData = false;
                    break;
                }
            }

            if (isPossibleData)
            {
                //합성 가능한 아이템을 생성
                NetworkObject crateItem = Instantiate(craftData.crateItem, crateItmeSpawnTrs.position ,Quaternion.identity);
                crateItem.Spawn(true);
                //올려둔 아이템 제거하고
                TableItemRemoveServerRpc();
                //나가기
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
        onTableItem.Clear(); // 기존 목록을 지우고 새로 시작

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
