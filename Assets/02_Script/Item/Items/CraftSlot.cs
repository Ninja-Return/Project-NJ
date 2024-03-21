using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour
{

    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;

    public void SetUp(ItemDataSO data)
    {

        text.text = data.itemName;
        image.sprite = data.slotData.slotSprite;

    }

}
