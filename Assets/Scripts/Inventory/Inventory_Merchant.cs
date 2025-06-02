using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Merchant : Inventory
{
    Inventory_Player playerInventory;

    [SerializeField] Data_ItemListSO shopData;
    [SerializeField] int minItemsAmount = 4;

    protected override void Awake()
    {
        base.Awake();
        FillShopList();
    }

    public void TryBuyItem(Inventory_Item itemToBuy, bool buyFullStack)
    {
        int amountToBuy = buyFullStack ? itemToBuy.stackSize : 1;

        for (int i = 0; i < amountToBuy; i++)
        {
            if (playerInventory.gold < itemToBuy.buyPrice)
                return;

            var newItem = new Inventory_Item(itemToBuy.itemData);

            if (itemToBuy.itemData.type == ItemType.Material)
                playerInventory.storage.AddMaterialToStash(newItem);

            else if (playerInventory.CanAddItemToInventory(itemToBuy))
                playerInventory.AddItemToInventory(newItem);

            RemoveItemFromInventory(itemToBuy);
            playerInventory.gold -= itemToBuy.buyPrice;
        }

        UpdateUI();
    }

    public void SellItem(Inventory_Item itemToSell, bool sellFullStack)
    {
        int amountToSell = sellFullStack ? itemToSell.stackSize : 1;

        for (int i = 0;i < amountToSell; i++)
        {
            playerInventory.gold += itemToSell.sellPrice;
            playerInventory.RemoveItemFromInventory(itemToSell);
        }

        UpdateUI();
    }
    public void SetInventory(Inventory_Player playerInventory) => this.playerInventory = playerInventory;

    public void FillShopList()
    {
        itemList.Clear();
        var possibleItems = new List<Inventory_Item>();

        foreach (var itemData in shopData.itemList)
        {
            int randomizedStack = Random.Range(itemData.minShopStackSize, itemData.maxStackSize+1);
            int finalStack = Mathf.Clamp(randomizedStack, 1, itemData.maxStackSize);

            var itemToAdd = new Inventory_Item(itemData);
            itemToAdd.SetStacks(finalStack);

            possibleItems.Add(itemToAdd);
        }

        int randomItemAmount = Random.Range(minItemsAmount, maxInventorySize+1);
        int finalItemAmount = Mathf.Clamp(randomItemAmount, 1, possibleItems.Count);

        for (int i = 0; i < finalItemAmount; i++)
        {
            var randomIndex = Random.Range(0, possibleItems.Count);
            var item = possibleItems[randomIndex];

            if (CanAddItemToInventory(item))
            {
                possibleItems.RemoveAt(randomIndex);
                AddItemToInventory(item);
            }
        }

        UpdateUI();
    }
}
