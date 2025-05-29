using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipSlot : UI_ItemSlot
{
    [field: SerializeField] public ItemType slotType { get; private set; }

    private void OnValidate()
    {
        gameObject.name = $"UI_EquipSlot - {slotType}";
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        inventory.UnequipItem(itemInSlot);

        ui.itemTooltip.ShowTooltip(false, null);
    }
}
