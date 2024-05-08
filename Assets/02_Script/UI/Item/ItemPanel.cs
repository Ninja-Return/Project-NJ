using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text itemText;
    [SerializeField] private TMP_Text priceText;
    public ItemRoot Item { get; private set; }

    private RectTransform rectTransform; 
    private Camera cam;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cam = Camera.main;
    }

    public void UpdatePanelPosition(Vector3 screenPosition)
    {
        rectTransform.position = screenPosition;
    }

    public void SetPanelVisibility(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void SetItem(ItemRoot itemRoot)
    {
        Item = itemRoot;
        itemText.text = itemRoot.data.itemName;
        priceText.text = $"АЁАн : {itemRoot.data.price}";
    }
}
