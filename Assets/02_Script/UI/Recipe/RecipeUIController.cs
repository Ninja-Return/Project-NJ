using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeUIController : MonoBehaviour
{

    [SerializeField] private RecipePanel panelPrefab;
    [SerializeField] private Transform panelRoot;

    private void Start()
    {
        
        var list = Resources.LoadAll<CraftData>("CraftData");

        foreach (var item in list)
        {

            var panel = Instantiate(panelPrefab, panelRoot);
            panel.Setting(item);

        }

    }

}
