using TMPro;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftMaterialSlot : MonoBehaviour
{
    [SerializeField] Image materialIcon;
    [SerializeField] TextMeshProUGUI materialName;
    [SerializeField] TextMeshProUGUI materialValue;

    public void SetupMaterialSlot(Data_ItemSO itemData, int availableAmount, int requiredAmount)
    {
        materialIcon.sprite = itemData.icon;
        materialName.text = $"{itemData.itemName}";
        string availableStr = availableAmount < requiredAmount ? 
            $"<color=#FF0000>{availableAmount}</color>" : availableAmount.ToString();
        materialValue.text = $"{availableStr}/{requiredAmount}";
    }
}
