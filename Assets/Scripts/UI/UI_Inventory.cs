using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    UI_ItemSlot[] uiItemSlots;
    Inventory inventory;

    private void Awake()
    {
        uiItemSlots = GetComponentsInChildren<UI_ItemSlot>();
        inventory = FindFirstObjectByType<Inventory>();
        inventory.OnInventoryChange += UpdateInventorySlots;
        UpdateInventorySlots();
    }

    void UpdateInventorySlots()
    {
        List<Inventory_Item> itemList = inventory.itemList;

        for (int i = 0; i < uiItemSlots.Length; i++)
        {
            if (i < itemList.Count)
            {
                uiItemSlots[i].UpdateSlot(itemList[i]);
            }
            else
            {
                uiItemSlots[i].UpdateSlot(null);
            }
        }
    }
}
