using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipePanel : MonoBehaviour
{

    [SerializeField] private CraftSlot resultIcon;
    [SerializeField] private CraftSlot recipeUIPrefab;
    [SerializeField] private Transform recipeParent;

    public void Setting(CraftData data)
    {

        foreach(var item in data.materialsItems)
        {

            Instantiate(recipeUIPrefab, recipeParent).SetUp(item);

        }

        resultIcon.SetUp(data.resultData);

    }

}
