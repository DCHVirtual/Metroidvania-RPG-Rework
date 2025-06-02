using UnityEngine;

public class UI_Craft : MonoBehaviour
{
    [SerializeField] UI_ItemSlotParent inventoryParent;

    Inventory_Player inventory;
    UI_CraftPreview craftPreviewUI;
    UI_CraftBookSlot[] craftSlots;
    UI_CraftCategoryButton[] craftCategoryButtons;

    public void SetupCraftUI(Inventory_Storage storage)
    {
        inventory = storage.playerInventory;
        craftPreviewUI = GetComponentInChildren<UI_CraftPreview>();
        craftPreviewUI.SetupCraftPreview(storage);
        SetupCraftCategoryButtons();

        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    void SetupCraftCategoryButtons()
    {
        craftSlots = GetComponentsInChildren<UI_CraftBookSlot>(true);
        craftCategoryButtons = GetComponentsInChildren<UI_CraftCategoryButton>(true);

        foreach (var craftSlot in craftSlots)
            craftSlot.gameObject.SetActive(false);

        foreach (var button in craftCategoryButtons)
            button.SetCraftSlots(craftSlots);
    }

    void UpdateUI() => inventoryParent.UpdateSlots(inventory.itemList);
}
