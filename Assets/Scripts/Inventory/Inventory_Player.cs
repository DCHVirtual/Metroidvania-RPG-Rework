using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory
{
    public int gold = 10000;

    Entity_Stats stats;
    Player player;
    public Inventory_Storage storage { get; private set; }
    [field: SerializeField] public List<Inventory_EquipmentSlot> equipList { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stats = GetComponent<Entity_Stats>();
        player = FindFirstObjectByType<Player>();
        storage = FindFirstObjectByType<Inventory_Storage>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindAnyItem(item.itemData);
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

    void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot, bool swap = false)
    {
        float currentHealthPercent = player.health.GetHealthPercent();

        slot.EquipItem(itemToEquip);
        slot.equipedItem.AddModifiers(stats);
        slot.equipedItem.AddItemEffect(player);

        player.health.SetHealthPercent(currentHealthPercent);

        RemoveItemFromInventory(itemToEquip);

        UpdateUI();
    }

    public void UnequipItem(Inventory_Item itemToUnequip)
    {
        if (!CanAddItemToInventory(itemToUnequip))
            return;

        float currentHealthPercent = player.health.GetHealthPercent();

        foreach (var slot in equipList)
        {
            if (slot.equipedItem.ID.Equals(itemToUnequip.ID))
            {
                slot.equipedItem.RemoveModifiers(stats);
                slot.equipedItem.itemEffect?.Unsubscribe();
                slot.EquipItem(null);
                AddItemToInventory(itemToUnequip);
                break;
            }
        }

        player.health.SetHealthPercent(currentHealthPercent);
    }
}
