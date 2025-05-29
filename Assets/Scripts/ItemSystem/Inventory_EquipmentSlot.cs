using System;
using UnityEngine;

[Serializable]
public class Inventory_EquipmentSlot
{
    [field: SerializeField] public ItemType type { get; private set; }
    [field: SerializeField] public Inventory_Item equipedItem { get; private set; }

    public bool HasItem() => equipedItem != null && equipedItem.itemData != null;

    public void EquipItem(Inventory_Item itemToEquip)
    {
        equipedItem = itemToEquip;
    }
}
