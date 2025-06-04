using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveable
{
    [SerializeField] protected Data_ItemListSO itemDataBase;

    public event Action OnInventoryChange;

    public int maxInventorySize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    protected virtual void Awake()
    {

    }

    public void TryUseItem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = FindItem(itemToUse);

        if (consumable != null)
        {
            consumable.itemEffect.ExecuteEffect();

            consumable.RemoveStack();

            if (consumable.stackSize == 0)
                RemoveItemFromInventory(consumable);

            UpdateUI();
        }
    }

    public bool CanAddItemToInventory(Inventory_Item itemToAdd)
    {
        if (itemList.Count < maxInventorySize)
            return true;

        var allMatchingItems = FindAllItems(itemToAdd.itemData)
            .Where(item => item.CanAddStack()).ToList();

        if (allMatchingItems.Count > 0)
            return true;

        return false;
    }

    public void AddItemToInventory(Inventory_Item itemToAdd)
    {
        Inventory_Item itemInInventory = FindAllItems(itemToAdd.itemData)
            .Where(item => item.CanAddStack())
            .FirstOrDefault();

        if (itemInInventory != null)
            itemInInventory.AddStack();
        else
            itemList.Add(itemToAdd);

        UpdateUI();
    }


    public void RemoveItemFromInventory(Inventory_Item item)
    {
        var itemInInventory = FindItem(item);

        if (itemInInventory == null) return;

        if (itemInInventory.stackSize > 1)
            itemInInventory.RemoveStack();
        else
            itemList.Remove(item);

        UpdateUI();
    }

    public Inventory_Item FindItem(Inventory_Item itemToFind)
    {
        return itemList.Find(item => item == itemToFind);
    }

    public Inventory_Item FindAnyItem(Data_ItemSO itemData)
    {
        return itemList.Find(item => item.itemData == itemData);
    }

    public List<Inventory_Item> FindAllItems(Data_ItemSO itemData)
    {
        return itemList.FindAll(item => item.itemData == itemData);
    }

    public void UpdateUI() => OnInventoryChange?.Invoke();

    public virtual void LoadData(GameData data)
    {
        
    }

    public virtual void SaveData(ref GameData data)
    {
        
    }

    protected void SaveItem(Inventory_Item item, ref SerializableDictionary<string, int> itemDict)
    {
        if (item != null && item.itemData != null)
        {
            string saveID = item.itemData.saveID;

            if (itemDict.ContainsKey(saveID) == false)
                itemDict[saveID] = 0;

            itemDict[saveID] += item.stackSize;
        }
    }
}
