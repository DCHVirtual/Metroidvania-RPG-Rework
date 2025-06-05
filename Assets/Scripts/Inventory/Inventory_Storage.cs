using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory_Storage : Inventory
{
    public Inventory_Player playerInventory { get; private set; }
    public List<Inventory_Item> materialList { get; private set; } = new List<Inventory_Item>();
    public void SetInventory(Inventory_Player inventory) => playerInventory = inventory; 

    #region Crafting
    public void CraftItem(Inventory_Item itemToCraft)
    {
        foreach (var neededMaterial in itemToCraft.itemData.craftRecipe)
            ConsumeNeededMaterials(neededMaterial);

        playerInventory.AddItemToInventory(itemToCraft);
    }

    private void ConsumeNeededMaterials(Inventory_Item neededMaterial)
    {
        List<Inventory_Item>[] itemLists = { materialList, playerInventory.itemList, itemList };
        int requiredAmount = neededMaterial.stackSize;

        foreach (var list in itemLists)
        {
            int consumedAmount = ConsumeItemsFrom(list, neededMaterial, requiredAmount);
            requiredAmount -= consumedAmount;

            if (requiredAmount == 0)
                break;
        }
    }

    public int ConsumeItemsFrom(List<Inventory_Item> list, Inventory_Item itemToConsume, int requiredAmount)
    {
        int totalConsumedAmount = 0;

        for (int i = list.Count-1; i >= 0; i--)
        {
            var item = list[i];
            if (item.itemData != itemToConsume.itemData)
                continue;

            int consumedAmount = item.RemoveStacks(requiredAmount);
            totalConsumedAmount += consumedAmount;
            requiredAmount -= consumedAmount;

            if (item.stackSize == 0)
                list.RemoveAt(i);

            if (requiredAmount == 0)
                break;
        }

        return totalConsumedAmount;
    }
    public bool HasEnoughToCraft(Inventory_Item itemToCraft)
    {
        foreach (var material in itemToCraft.itemData.craftRecipe)
        {
            if (GetAvailableAmountOf(material.itemData) < material.stackSize)
                return false;
        }

        return true;
    }
    public int GetAvailableAmountOf(Data_ItemSO requiredItem)
    {
        var totalItemCount
            = materialList.Concat(playerInventory.itemList).Concat(itemList)
            .Where(item => item.itemData == requiredItem)
            .Select(item => item.stackSize).Sum();

        return totalItemCount;
    }
    #endregion

    #region Storage Transfer Functions
    public void AddMaterialToStash(Inventory_Item itemToAdd)
    {
        var stackableItem = StackableInStash(itemToAdd);

        if (stackableItem != null)
            stackableItem.AddStack();
        else
            materialList.Add(itemToAdd);

        materialList = materialList.OrderBy(t => t.itemData.itemName).ToList();

        UpdateUI();
    }

    public Inventory_Item StackableInStash(Inventory_Item itemToAdd)
    {
        return materialList.FindAll(item => 
            item.itemData == itemToAdd.itemData && item.CanAddStack())
            .FirstOrDefault();
    }

    public void FromPlayerToStorage(Inventory_Item item, bool fullStack)
    {
        int transferAmount = fullStack ? item.stackSize : 1;

        for (int i = 0; i < transferAmount; i++)
        {
            if (CanAddItemToInventory(item))
            {
                AddItemToInventory(new Inventory_Item(item.itemData));
                playerInventory.RemoveItemFromInventory(item);
            }
        }

        UpdateUI();
    }

    public void FromStorageToPlayer(Inventory_Item item, bool fullStack)
    {
        int transferAmount = fullStack ? item.stackSize : 1;

        for (int i = 0; i < transferAmount; i++)
        {
            if (playerInventory.CanAddItemToInventory(item))
            {
                playerInventory.AddItemToInventory(new Inventory_Item(item.itemData));
                RemoveItemFromInventory(item);
            }
        }

        UpdateUI();
    }
    #endregion

    #region Save / Load functions
    public override void SaveData(ref GameData data)
    {
        data.storageItems.Clear();
        data.storageMaterials.Clear();

        foreach (var item in itemList)
            SaveItem(item, ref data.storageItems);

        foreach (var item in materialList)
            SaveItem(item, ref data.storageMaterials);
    }

    

    public override void LoadData(GameData data)
    {
        itemList.Clear();
        materialList.Clear();

        foreach (var item in data.storageItems)
            LoadItem(item);

        foreach (var item in data.storageMaterials)
            LoadItem(item);

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
        {
            if (itemToLoad.itemData.type == ItemType.Material)
                AddMaterialToStash(itemToLoad);
            else
                AddItemToInventory(itemToLoad);
        }
    }
    #endregion
}
