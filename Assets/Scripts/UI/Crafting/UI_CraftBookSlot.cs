using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftBookSlot : MonoBehaviour
{
    Data_ItemSO itemToCraft;

    [SerializeField] UI_CraftPreview craftPreview;
    [SerializeField] TextMeshProUGUI craftItemName;
    [SerializeField] Image craftItemIcon;

    public void SetupButton(Data_ItemSO craftData)
    {
        itemToCraft = craftData;
        craftItemIcon.sprite = craftData.icon;
        craftItemName.text = craftData.itemName;
    }

    public void UpdateCraftPreview() => craftPreview.UpdateCraftPreview(itemToCraft);
}
