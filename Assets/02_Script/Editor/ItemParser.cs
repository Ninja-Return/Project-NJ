using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FD.Core.Editors;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemParser : FAED_GoogleFormParser
{

    private Button parsingBtn;

    [MenuItem("Custom/ItemParser")]
    private static void CreateAndShow()
    {

        var window = CreateWindow<ItemParser>();
        window.Init();

    }

    private void Init()
    {

        parsingBtn = new Button(HandleBtnClick);

        parsingBtn.text = "Parsing";

        rootVisualElement.Add(parsingBtn);

    }

    private void HandleBtnClick()
    {

        GetFromData("1f5oYMm3rFL2KL1lEe4cdn7_vbiytvcjQ604bXH8_jts", "0", EndParsing);
        
    }

    private void EndParsing(bool success, string data)
    {

        var strs = data.Split('\n').ToList();
        strs.RemoveAt(0);

        List<ItemParsingData> datas = new List<ItemParsingData>();

        foreach (var item in strs)
        {

            var splits = item.Split(',');
            datas.Add(new ItemParsingData(splits[0], splits[1], splits[2], splits[3]));

        }

        foreach(var item in datas)
        {

            if(File.Exists(Application.dataPath 
                + $"/05_SO/Item/{item.poolingKey}_ItemData.asset") 
                &&
                File.Exists(Application.dataPath 
                + $"/05_SO/Slot/{item.poolingKey}_SlotData.asset"))
            {

                var itemData = AssetDatabase.LoadAssetAtPath<ItemDataSO>($"Assets/05_SO/Item/{item.poolingKey}_ItemData.asset");
                var slotData = AssetDatabase.LoadAssetAtPath<SlotData>($"Assets/05_SO/Slot/{item.poolingKey}_SlotData.asset");

                itemData.itemName = item.itemName;
                itemData.price = item.price;
                slotData.poolingName = item.poolingKey;
                slotData.slotExplanation = item.itemExp;

                EditorUtility.SetDirty(itemData);
                EditorUtility.SetDirty(slotData);

            }
            else
            {

                var itemData = ScriptableObject.CreateInstance<ItemDataSO>();
                var slotData = ScriptableObject.CreateInstance<SlotData>();

                itemData.itemName = item.itemName;
                itemData.price = item.price;
                itemData.slotData = slotData;
                
                slotData.poolingName = item.poolingKey;
                slotData.slotExplanation = item.itemExp;

                AssetDatabase.CreateAsset(itemData, 
                    $"Assets/05_SO/Item/{item.poolingKey}_ItemData.asset");

                AssetDatabase.CreateAsset(slotData, 
                    $"Assets/05_SO/Slot/{item.poolingKey}_SlotData.asset");

                AssetDatabase.SaveAssets();

                EditorUtility.SetDirty(itemData);
                EditorUtility.SetDirty(slotData);

            }

        }

    }



    private struct ItemParsingData
    {

        public ItemParsingData(string itemName, string poolingKey, string itemExp, string price)
        {

            this.itemName = itemName;
            this.poolingKey = poolingKey;
            this.itemExp = itemExp;
            this.price = int.Parse(price);

        }

        public string itemName;
        public string poolingKey;
        public string itemExp;
        public int price;

        public override string ToString()
        {
            return $"{itemName} : {poolingKey} : {itemExp}";
        }

    }

}
