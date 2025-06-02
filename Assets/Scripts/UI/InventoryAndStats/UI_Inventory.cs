using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    UI_ItemSlot[] uiItemSlots;
    UI_EquipSlot[] uiEquipSlots;
    UI_StatSlot[] uiStatSlots;
    Inventory_Player inventory;
    Player player;

    [SerializeField] UI_ItemSlotParent uiItemSlotParent;
    [SerializeField] Transform uiEquipSlotParent;
    [SerializeField] Transform uiStatSlotParent;
    [SerializeField] Slider uiHealthBar;


    private void Awake()
    {
        uiItemSlots = uiItemSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();
        uiStatSlots = uiStatSlotParent.GetComponentsInChildren<UI_StatSlot>();
        inventory = FindFirstObjectByType<Inventory_Player>();
        player = FindFirstObjectByType<Player>();
        inventory.OnInventoryChange += UpdateUI;
        UpdateFontSize();
    }

    private void Start()
    {
        UpdateUI();
    }

    void UpdateFontSize()
    {
        float desiredSize = uiStatSlots[4].GetFontSize();
        foreach (var slot in uiStatSlots)
            slot.SetFontSize(desiredSize);
    }
    public void UpdateUI()
    {
        uiHealthBar.value = player.health.GetHealthPercent();
        uiItemSlotParent.UpdateSlots(inventory.itemList);
        UpdateEquipmentSlots();
        UpdateStatSlots();
    }

    void UpdateStatSlots()
    {
        foreach (var slot in uiStatSlots)
        {
            slot.UpdateStatValue();
            
        }
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
}
