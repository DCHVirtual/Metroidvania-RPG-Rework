using UnityEngine;

public class UI_Merchant : MonoBehaviour
{

    Inventory_Player playerInventory;
    Inventory_Merchant merchant;

    [SerializeField] UI_ItemSlotParent merchantSlotParent;
    [SerializeField] UI_ItemSlotParent inventorySlotParent;
    [SerializeField] UI_EquipSlotParent equipSlotParent;

    public void SetupMerchantUI(Inventory_Merchant merchant, Inventory_Player playerInventory)
    {
        this.merchant = merchant;
        this.playerInventory = playerInventory;

        playerInventory.storage.OnInventoryChange += UpdateSlotsUI;
        playerInventory.OnInventoryChange += UpdateSlotsUI;
        merchant.OnInventoryChange += UpdateSlotsUI;
        UpdateSlotsUI();

        var merchSlots = GetComponentsInChildren<UI_MerchantSlot>();
        foreach (var slot in merchSlots)
            slot.SetupMerchantUI(merchant);
    }

    void UpdateSlotsUI()
    {
        if (playerInventory == null) return;

        inventorySlotParent.UpdateSlots(playerInventory.itemList);
        merchantSlotParent.UpdateSlots(merchant.itemList);
        equipSlotParent.UpdateEquipmentSlots(playerInventory.equipList);
    }
}
