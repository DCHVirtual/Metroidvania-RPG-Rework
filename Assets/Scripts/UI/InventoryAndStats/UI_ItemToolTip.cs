using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemType;
    [SerializeField] TextMeshProUGUI itemInfo;

    [SerializeField] TextMeshProUGUI itemPrice;
    [SerializeField] Transform merchantInfo;

    public void ShowTooltip(bool show, RectTransform hoverRect, Inventory_Item item, bool buyPrice = false, bool showMerchInfo = false)
    {
        itemName.text = item.itemData.itemName;
        itemType.text = item.itemData.type.ToString();
        itemInfo.text = ui.GetItemInfo(item);

        int price = buyPrice ? item.buyPrice : item.sellPrice;
        string text = buyPrice ? "Price: " : "Sell for: ";

        itemPrice.text = $"{text}{price}g";

        merchantInfo.gameObject.SetActive(showMerchInfo);


        base.ShowTooltip(show, hoverRect);
    }
}
