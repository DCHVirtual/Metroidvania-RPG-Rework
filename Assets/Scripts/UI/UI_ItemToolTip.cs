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
        base.ShowTooltip(show, hoverRect);

        itemName.text = item.itemData.itemName;
        itemType.text = item.itemData.type.ToString();
        itemInfo.text = GetItemInfo(item);
    }

    public string GetItemInfo(Inventory_Item item)
    {
        if (item.itemData.type == ItemType.Material)
            return "Used for crafting.";

        StringBuilder sb = new StringBuilder();

        foreach (var mod in item.modifiers)
            sb.AppendLine($"+{mod.value}{PercentString(mod.statType)} {GetStatNameByType(mod.statType)}");

        return sb.ToString();
    }

    string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.Armor:
            case StatType.Evasion:
            case StatType.Agility:
            case StatType.Intelligence:
            case StatType.Vitality:
            case StatType.Damage:
            case StatType.Strength: return type.ToString();
            case StatType.MaxHealth: return "Max Health";
            case StatType.HealthRegen: return "Health Regeneration";
            case StatType.AttackSpeed: return "Attack Speed";
            case StatType.CritChance: return "Critical Chance";
            case StatType.CritDamage: return "Critical Damage";
            case StatType.ArmorReduction: return "Armor Reduction";
            case StatType.FireDamage: return "Fire Damage";
            case StatType.IceDamage: return "Ice Damage";
            case StatType.LightningDamage: return "Lightning Damage";
            case StatType.IceResistance: return "Ice Resistance";
            case StatType.FireResistance: return "Fire Resistance";
            case StatType.LightningResistance: return "Lightning Resistance";
            default: return "";
        }
    }

    string PercentString(StatType type)
    {
        switch (type)
        {
            case StatType.CritChance:
            case StatType.CritDamage:
            case StatType.ArmorReduction:
            case StatType.IceResistance:
            case StatType.FireResistance:
            case StatType.LightningResistance:
            case StatType.AttackSpeed:
            case StatType.Evasion:
                return "%";
            default:
                return "";
        }
    }
}
