using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerItemRader : NetworkBehaviour
{
    [SerializeField] private float raderRadius = 5f;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private ItemPanel itemPanelPrefab;
    [SerializeField] private Canvas canvas;

    private Camera cam;
    private Canvas itemPanelCanvas;
    private List<ItemPanel> activeItemPanels = new List<ItemPanel>();

    private void Start()
    {
        if (!IsOwner) return;

        cam = Camera.main;
        itemPanelCanvas = Instantiate(canvas);
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (cam == null)
        {
            cam = Camera.main;
        }

        Collider[] items = Physics.OverlapSphere(cam.transform.position, raderRadius, itemLayer);

        List<ItemRoot> existingItems = GetExistingItems(); // 이미 생성된 아이템을 추적하기 위한 리스트
        List<ItemRoot> raderItems = GetRaderItems(items); // 감지 범위 안의 아이템을 추적하기 위한 리스트

        InstantiatePanels(existingItems, raderItems);
        UpdateExistingPanels(existingItems, raderItems);
    }

    private List<ItemRoot> GetExistingItems()
    {
        List<ItemRoot> existingItems = new List<ItemRoot>();

        foreach (ItemPanel panel in activeItemPanels)
        {
            existingItems.Add(panel.Item);
        }

        return existingItems;
    }

    private List<ItemRoot> GetRaderItems(Collider[] items)
    {
        List<ItemRoot> raderItems = new List<ItemRoot>();

        foreach (Collider item in items)
        {
            if (item.TryGetComponent<ItemRoot>(out ItemRoot itemRoot))
            {
                raderItems.Add(itemRoot);
            }
        }

        return raderItems;
    }

    private void InstantiatePanels(List<ItemRoot> existingItems, List<ItemRoot> raderItems)
    {
        foreach (ItemRoot itemRoot in raderItems)
        {
            if (!existingItems.Contains(itemRoot))
            {
                Vector3 itemScreenPos = cam.WorldToScreenPoint(itemRoot.transform.position);

                if (IsItemBehindCamera(itemRoot))
                {
                    itemScreenPos += Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up) * 50f;

                    ItemPanel panel = Instantiate(itemPanelPrefab, itemScreenPos, Quaternion.identity, itemPanelCanvas.transform);
                    panel.SetItem(itemRoot); // 아이템 정보 설정
                    panel.transform.TVEffect(true);

                    // 생성된 패널과 연관된 아이템 추가
                    activeItemPanels.Add(panel);
                    existingItems.Add(itemRoot);
                }
            }
        }
    }

    private bool IsItemBehindCamera(ItemRoot itemRoot)
    {
        Vector3 fromItemToCamera = cam.transform.position - itemRoot.transform.position;
        Vector3 cameraForward = cam.transform.forward;
        float dot = Vector3.Dot(cameraForward, fromItemToCamera);

        return dot < 0f;
    }

    private void UpdateExistingPanels(List<ItemRoot> existingItems, List<ItemRoot> raderItems)
    {
        for (int i = activeItemPanels.Count - 1; i >= 0; i--)
        {
            ItemPanel panel = activeItemPanels[i];
            if (!raderItems.Contains(panel.Item))
            {
                Destroy(panel.gameObject);
                activeItemPanels.RemoveAt(i);
            }
            else
            {
                Vector3 itemScreenPos = cam.WorldToScreenPoint(panel.Item.transform.position);

                if (IsItemBehindCamera(panel.Item))
                {
                    panel.SetPanelVisibility(true);
                    panel.UpdatePanelPosition(itemScreenPos);
                }
                else
                {
                    panel.SetPanelVisibility(false);
                }
            }
        }
    }
}
