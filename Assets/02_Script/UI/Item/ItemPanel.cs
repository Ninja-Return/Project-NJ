using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text itemText;
    [SerializeField] private TMP_Text usingText;
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
        string isUsing = itemRoot.data.itemType == EnumList.ItemUseType.Possible ? "0" : "X";
        Item = itemRoot;

        itemText.text = itemRoot.data.itemName;
        usingText.text = $"사용 가능 여부 : {isUsing}";
        priceText.text = $"가격 : {itemRoot.data.price}";
    }
}
