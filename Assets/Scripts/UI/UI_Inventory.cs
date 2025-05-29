using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    UI_ItemSlot[] uiItemSlots;
    UI_EquipSlot[] uiEquipSlots;
    Inventory_Player inventory;

    [SerializeField] Transform uiItemSlotParent;
    [SerializeField] Transform uiEquipSlotParent;


    private void Awake()
    {
        uiItemSlots = uiItemSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();
        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }

    void UpdateUI()
    {
        UpdateInventorySlots();
        UpdateEquipmentSlots();
    }

    void UpdateEquipmentSlots()
    {
        var equipList = inventory.equipList;

        for (int i = 0; i < uiEquipSlots.Length; i++)
        {
            if (!equipList[i].HasItem())
                uiEquipSlots[i].UpdateSlot(null);
            else
                uiEquipSlots[i].UpdateSlot(equipList[i].equipedItem);
        }
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
