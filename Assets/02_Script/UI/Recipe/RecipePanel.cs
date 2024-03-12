using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipePanel : MonoBehaviour
{

    [SerializeField] private Image resultIcon;
    [SerializeField] private Image recipeUIPrefab;
    [SerializeField] private Transform recipeParent;

    public void Setting(CraftData data)
    {

        foreach(var item in data.materialsItems)
        {

            Instantiate(recipeUIPrefab, recipeParent).sprite = item.slotData.slotSprite;

        }

        resultIcon.sprite = data.resultData.slotData.slotSprite;

    }

}
