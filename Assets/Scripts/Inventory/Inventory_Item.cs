using System;
using UnityEngine;

[Serializable]
public class Inventory_Item
{
    [field: SerializeField] public Data_ItemSO itemData { get; private set; }
    public Data_ItemEffectSO itemEffect { get; private set; }
    [field: SerializeField] public int stackSize { get; private set; }
    public Guid ID { get; private set; }

    public ItemModifier[] modifiers { get; private set; }

    public int buyPrice { get; private set; }
    public int sellPrice { get; private set; }

    public Inventory_Item(Data_ItemSO itemData)
    {
        stackSize = 1;
        this.itemData = itemData;
        itemEffect = itemData.effect;
        buyPrice = itemData.price;
        sellPrice = (int)(0.35f * buyPrice);
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

    public void AddItemEffect(Player player) => itemEffect?.Subscribe(player);
    public void RemoveItemEffect() => itemEffect?.Unsubscribe();
    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;

    public int RemoveStacks(int amount)
    {
        int startSize = stackSize;

        stackSize -= Mathf.Min(amount, stackSize);

        return startSize - stackSize; //returns amount removed
    }

    public void SetStacks(int size)
    {
        stackSize = Mathf.Clamp(size,0, itemData.maxStackSize);
    }
}
