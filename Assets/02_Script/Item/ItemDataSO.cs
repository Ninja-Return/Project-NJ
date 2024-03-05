using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Data")]
public class ItemDataSO : ScriptableObject, IEquatable<ItemDataSO>
{

    [field: SerializeField] public string itemName { get; private set; }
    [field: SerializeField] public SlotData slotData { get; private set; }

    public bool Equals(ItemDataSO other)
    {

        return other.itemName == itemName;

    }

}