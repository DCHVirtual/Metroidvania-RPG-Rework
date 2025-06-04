using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory_Player : Inventory
{
    public int gold = 10000;

    Player_Stats stats;
    Player player;
    public Inventory_Storage storage { get; private set; }
    [field: SerializeField] public List<Inventory_EquipmentSlot> equipList { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stats = GetComponent<Player_Stats>();
        player = FindFirstObjectByType<Player>();
        storage = FindFirstObjectByType<Inventory_Storage>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        //var inventoryItem = FindAnyItem(item.itemData);
        var matchingSlots = equipList.FindAll(slot => slot.type == item.itemData.type);

        for (int i = 0; i < matchingSlots.Count; i++)
        {
            var slot = matchingSlots[i];
            if (!slot.HasItem())
            {
                EquipItem(item, slot);
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

    public override void SaveData(ref GameData data)
    {
        data.gold = gold;
        data.inventory.Clear();
        data.equipment.Clear();

        foreach (var item in itemList)
            SaveItem(item, ref data.inventory);

        foreach (var slot in equipList.Where(s => s.HasItem()).ToArray())
            data.equipment.Add(slot.equipedItem.itemData.saveID);
    }

    public override void LoadData(GameData data)
    {
        gold = data.gold;
        itemList.Clear();

        foreach (var item in data.inventory)
            LoadItem(item);

        foreach (var itemID in data.equipment) 
            TryEquipItem(new Inventory_Item(itemDataBase.GetItemData(itemID)));

        UpdateUI();
    }

    protected void LoadItem(KeyValuePair<string, int> item)
    {
        string saveID = item.Key;
        int stackSize = item.Value;

        Data_ItemSO itemData = itemDataBase.GetItemData(saveID);

        if (itemData == null)
        {
            Debug.LogWarning("Item not found: " + saveID);
            return;
        }

        Inventory_Item itemToLoad = new Inventory_Item(itemData);

        for (int i = 0; i < stackSize; i++)
            AddItemToInventory(itemToLoad);
    }
}
