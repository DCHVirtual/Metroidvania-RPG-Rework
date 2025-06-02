using TMPro;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTip
{
    Player_Stats playerStats;
    TextMeshProUGUI statToolTipText;

    protected override void Awake()
    {
        base.Awake();
        playerStats = FindAnyObjectByType<Player_Stats>();
        statToolTipText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowTooltip(bool show, RectTransform hoverRect, StatType statType)
    {
        base.ShowTooltip(show, hoverRect);
        statToolTipText.text = GetStatInfo(statType);
    }

    public string GetStatInfo(StatType statType)
    {
        switch(statType)
        {
            case StatType.Armor:
                return "Mitigates incoming damage." + 
                    "\nCapped at 85%." +
                    "\nCurrent mitigation: " + playerStats.GetArmorMitigation(0) * 100 + "%";
            case StatType.Evasion:
                return "Chance to dodge an attack completely.\nCapped at 85%";
            case StatType.Agility:
                return "Increases critical chance by 0.3% per point\n" +
                    "Increases evasion by 0.5% per point";
            case StatType.Intelligence:
                return "Increases elemental resistances by 0.5% per point.\n" +
                    "Increases elemental damage by 1 per point.\n" +
                    "Damage bonus not applied if other elements have 0 damage";
            case StatType.Vitality:
                return "Increases Max Health by 5 per point.\n" +
                    "Increases armor by 1 per point.";
            case StatType.Damage:
                return "Raw physical damage of your attacks";
            case StatType.Strength:
                return "Increases physical damage by 1 per point.\n" +
                    "Increases critical damage by 0.5% per point.";
            case StatType.MaxHealth: return "Determines your maximum total health";
            case StatType.HealthRegen: return "How much health you regenerate per second";
            case StatType.AttackSpeed: return "How fast you attack";
            case StatType.CritChance: return "Chance to perform a critical hit";
            case StatType.CritDamage: return "Damage done on critical hits";
            case StatType.ArmorReduction: return "Percent of armor ignored by your attacks";
            case StatType.FireDamage: return "Applies Burn Status, \ndealing extra damage per second.\n" +
                    "Only your highest elemental damage applies status.";
            case StatType.IceDamage: return "Applies Chilled status, \nslowing enemies attack and movement.\n" +
                    "Only your highest elemental damage applies status.";
            case StatType.LightningDamage: return "Applies shock status, \nenough charge causes lightning strike.\n" +
                    "Only your highest elemental damage applies status.";
            case StatType.ElementalDamage: return "1 point per maximum element damage, \n0.5 points from the rest.\n"+
                    "Improved by Intelligence.";
            case StatType.IceResistance: return "Reduces duration of enemy chill effects.";
            case StatType.FireResistance: return "Reduces incoming fire damage.";
            case StatType.LightningResistance: return "Reduces shock charge buildup.";
            default: return "";
        }
    }
}