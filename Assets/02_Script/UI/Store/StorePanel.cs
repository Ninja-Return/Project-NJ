using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text priceText;

    private StoreSystem storeSystem;
    private ItemDataSO curItem;
    private StoreUIController storeUI;
    private int priseItem;
    private string expText;

    private void Awake()
    {
        
        storeSystem = FindObjectOfType<StoreSystem>();

    }

    public void BuyClick()
    {

        if (storeSystem.Buy(curItem))
        {

            NetworkSoundManager.Play3DSound("BuyItem", transform.position, 0.01f, 20f);
            storeUI.StorePanelRefresh(curItem.itemName);
            Debug.Log("구입 성공");

        }

    }

    public void SetUp(StoreSellObject item, StoreUIController ui)
    {

        curItem = item.data;
        itemIcon.sprite = item.data.slotData.slotSprite;
        priceText.text = $"가격 : {item.price}";
        expText = item.expText;

        storeUI = ui;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        storeUI.SetExpText(expText);

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        storeUI.SetExpText(string.Empty);

    }

}
