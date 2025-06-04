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
    [SerializeField] UI_EquipSlotParent uiEquipSlotParent;
    [SerializeField] Transform uiStatSlotParent;
    [SerializeField] Slider uiHealthBar;


    private void Awake()
    {
        uiItemSlots = uiItemSlotParent.GetComponentsInChildren<UI_ItemSlot>(true);
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>(true);
        uiStatSlots = uiStatSlotParent.GetComponentsInChildren<UI_StatSlot>(true);
        inventory = FindFirstObjectByType<Inventory_Player>();
        player = FindFirstObjectByType<Player>();
        inventory.OnInventoryChange += UpdateUI;
        UpdateFontSize();
    }

    private void OnEnable()
    {
        if (inventory != null && player.stats != null)
            UpdateUI();
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
        if (player.health != null)
            uiHealthBar.value = player.health.GetHealthPercent();
        uiItemSlotParent.UpdateSlots(inventory.itemList);
        uiEquipSlotParent.UpdateEquipmentSlots(inventory.equipList);
        UpdateStatSlots();
    }

    void UpdateStatSlots()
    {
        foreach (var slot in uiStatSlots)
        {
            slot.UpdateStatValue();
            
        }
    }
}
