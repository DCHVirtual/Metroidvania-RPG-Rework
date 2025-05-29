using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryChange;

    public int maxInventorySize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    protected virtual void Awake()
    {

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

        OnInventoryChange?.Invoke();
    }

    public void RemoveItemFromInventory(Inventory_Item item)
    {
        itemList.Remove(item);
        OnInventoryChange?.Invoke();
    }

    public Inventory_Item FindItem(Data_ItemSO itemData)
    {
        return itemList.Find(item => item.itemData == itemData);
    }

    public List<Inventory_Item> FindAllItems(Data_ItemSO itemData)
    {
        return itemList.FindAll(item => item.itemData == itemData);
    }
}
