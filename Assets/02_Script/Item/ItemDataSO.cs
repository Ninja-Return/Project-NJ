using EnumList;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Data")]
public class ItemDataSO : ScriptableObject, IEquatable<ItemDataSO>
{

    [field: SerializeField] public string itemName { get; set; }
    [field: SerializeField] public SlotData slotData { get; set; }
    [field: SerializeField] public int price { get; set; }
    [field: SerializeField] public ItemUseType itemType { get; set; }

    public bool Equals(ItemDataSO other)
    {

        return other.itemName == itemName;

    }

}