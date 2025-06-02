using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    Inventory_Storage storage;
    Inventory_Player playerInventory;
    [SerializeField] UI_ItemSlotParent inventoryParent;
    [SerializeField] UI_ItemSlotParent storageParent;
    [SerializeField] UI_ItemSlotParent materialStashParent;
    public void SetupStorage(Inventory_Storage storage)
    {
        this.storage = storage;
        playerInventory = storage.playerInventory;
        storage.OnInventoryChange += UpdateUI;

        var storageSlots = GetComponentsInChildren<UI_StorageSlot>();

        foreach (var slot in storageSlots)
        {
            slot.SetStorage(storage);
        }
        
        UpdateUI();
    }

    private void OnEnable()
    {
        UpdateUI();
    }


    void UpdateUI()
    {
        inventoryParent.UpdateSlots(playerInventory.itemList);
        storageParent.UpdateSlots(storage.itemList);
        materialStashParent.UpdateSlots(storage.materialList);
    }
}
