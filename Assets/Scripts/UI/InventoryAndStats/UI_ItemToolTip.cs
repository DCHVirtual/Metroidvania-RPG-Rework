using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemType;
    [SerializeField] TextMeshProUGUI itemInfo;

    public void ShowTooltip(bool show, RectTransform hoverRect, Inventory_Item item)
    {
        itemName.text = item.itemData.itemName;
        itemType.text = item.itemData.type.ToString();
        itemInfo.text = ui.GetItemInfo(item);

        base.ShowTooltip(show, hoverRect);
    }
}
