using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MerchantSlot : UI_ItemSlot
{
    Inventory_Merchant merchant;
    public enum MerchantSlotType { MerchantSlot, PlayerSlot };
    public MerchantSlotType slotType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;

        bool rightButton = eventData.button == PointerEventData.InputButton.Right;
        bool leftButton = eventData.button == PointerEventData.InputButton.Left;

        if (slotType == MerchantSlotType.PlayerSlot)
        {
            bool fullStack = Input.GetKey(KeyCode.LeftControl);

            if (rightButton)
                merchant.SellItem(itemInSlot, fullStack);
            else if (leftButton)
            {
                base.OnPointerDown(eventData);
            }
        }
        else
        {
            if (leftButton)
                return;

            if (rightButton)
            {
                bool fullStack = Input.GetKey(KeyCode.LeftControl);
                merchant.TryBuyItem(itemInSlot, fullStack);
            }

        }

        ui.itemToolTip.ShowTooltip(false, null);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        ui.itemToolTip.ShowTooltip(true, rect, itemInSlot, slotType == MerchantSlotType.MerchantSlot, true);
    }

    public void SetupMerchantUI(Inventory_Merchant merchant) => this.merchant = merchant;

}
