using System;
using UnityEngine;

[Serializable]
public class Inventory_Item
{
    public Data_ItemSO itemData;
    public int stackSize = 1;
    public Guid ID { get; private set; }

    public ItemModifier[] modifiers { get; private set; }

    public Inventory_Item(Data_ItemSO itemData)
    {
        this.itemData = itemData;
        ID = Guid.NewGuid();
        modifiers = (itemData as Data_EquipmentSO)?.modifiers;
    }

    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.AddModifier(mod.value, itemData.itemName, ID);
        }
    }

    public void RemoveModifiers(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.RemoveModifier(ID);
        }
    }

    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
