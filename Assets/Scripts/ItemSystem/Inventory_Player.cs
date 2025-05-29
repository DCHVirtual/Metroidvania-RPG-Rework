using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory
{
    Entity_Stats stats;
    [field: SerializeField] public List<Inventory_EquipmentSlot> equipList { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stats = GetComponent<Entity_Stats>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item.itemData);
        var matchingSlots = equipList.FindAll(slot => slot.type == item.itemData.type);

        for (int i = 0; i < matchingSlots.Count; i++)
        {
            var slot = matchingSlots[i];
            if (!slot.HasItem())
            {
                EquipItem(inventoryItem, slot);
                return;
            }
            else if (i == matchingSlots.Count - 1)
            {
                RemoveItemFromInventory(item);
                UnequipItem(slot.equipedItem);
                EquipItem(item, slot);
            }
        }
    }

    void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        slot.EquipItem(itemToEquip);
        slot.equipedItem.AddModifiers(stats);

        RemoveItemFromInventory(itemToEquip);
    }

    public void UnequipItem(Inventory_Item itemToUnequip)
    {
        if (!CanAddItemToInventory(itemToUnequip))
            return;

        foreach (var slot in equipList)
        {
            if (slot.equipedItem.ID.Equals(itemToUnequip.ID))
            {
                slot.equipedItem.RemoveModifiers(stats);
                slot.EquipItem(null);
                AddItemToInventory(itemToUnequip);
                break;
            }
        }
    }
}
