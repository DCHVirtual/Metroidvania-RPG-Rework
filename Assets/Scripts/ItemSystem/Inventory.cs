using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryChange;

    public int maxInventorySize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    public bool CanAddItem(Inventory_Item itemToAdd)
    {
        if (itemList.Count < maxInventorySize)
            return true;

        if (FindItem(itemToAdd.itemData) != null)
            return true;

        return false;
    }

    public void AddItem(Inventory_Item item)
    {
        Inventory_Item itemInInventory = FindItem(item.itemData);

        if (itemInInventory != null)
            itemInInventory.AddStack();
        else
            itemList.Add(item);

        OnInventoryChange?.Invoke();
    }

    public Inventory_Item FindItem(Data_ItemSO itemData)
    {
        return itemList.Find(item => item.itemData == itemData && item.CanAddStack());
    }
}
