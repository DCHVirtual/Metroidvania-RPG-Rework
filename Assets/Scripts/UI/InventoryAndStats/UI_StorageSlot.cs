
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StorageSlot : UI_ItemSlot
{
    Inventory_Storage storage;

    enum StorageSlotType
    {
        Storage,
        Player
    }
    [SerializeField] StorageSlotType slotType;

    public void SetStorage(Inventory_Storage storage) => this.storage = storage;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        bool transferFullStack = Input.GetKey(KeyCode.LeftControl);

        if (slotType == StorageSlotType.Storage)
            storage.FromStorageToPlayer(itemInSlot, transferFullStack);
        else
            storage.FromPlayerToStorage(itemInSlot, transferFullStack);

        ui.itemToolTip.ShowTooltip(false, null);
    }

}
