using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rect;
    UI ui;
    Entity_Stats playerStats;

    [SerializeField] StatType statType;
    [SerializeField] TextMeshProUGUI statName;
    [SerializeField] TextMeshProUGUI statValue;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        ui = GetComponentInParent<UI>();
        playerStats = FindAnyObjectByType<Player>().GetComponent<Entity_Stats>();
    }

    private void OnValidate()
    {
        ui = GetComponentInParent<UI>();
        gameObject.name = "Stat - " + ui.GetStatNameByType(statType);
        statName.text = ui.GetStatNameByType(statType);
    }

    public float GetFontSize()
    {
        return statName.fontSize;
    }

    public void SetFontSize(float fontSize)
    {
        statName.enableAutoSizing = false;
        statValue.enableAutoSizing=false;
        statName.fontSize = fontSize;
        statValue.fontSize = fontSize;
    }

    public void UpdateStatValue()
    {
        Stat statToUpdate = playerStats.GetStatByType(statType);

        if (statToUpdate == null && statType != StatType.ElementalDamage)
            return;

        float value = 0;

        switch (statType)
        {
            //Primary Stats
            case StatType.Strength:
                value = playerStats.major.strength.GetValue(); 
                break;
            case StatType.Agility:
                value = playerStats.major.agility.GetValue(); 
                break;
            case StatType.Intelligence:
                value = playerStats.major.intelligence.GetValue(); 
                break;
            case StatType.Vitality:
                value = playerStats.major.vitality.GetValue(); 
                break;
            //Offense Stats
            case StatType.Damage:
                value = playerStats.GetBaseDamage();
                break;
            case StatType.AttackSpeed:
                value = playerStats.offense.attackSpeed.GetValue() * 100;
                break;
            case StatType.CritChance:
                value = playerStats.GetCritChance();
                break;
            case StatType.CritDamage:
                value = playerStats.GetCritDamage();
                break;
            case StatType.ArmorReduction:
                value = playerStats.GetArmorReduction() * 100;
                break;
            //Elemental Damage Stats
            case StatType.IceDamage:
                value = playerStats.offense.iceDamage.GetValue();
                break;
            case StatType.FireDamage:
                value = playerStats.offense.fireDamage.GetValue();
                break;
            case StatType.LightningDamage:
                value = playerStats.offense.lightningDamage.GetValue();
                break;
            case StatType.ElementalDamage:
                value = playerStats.GetElementalDamage(out ElementType _);
                break;
            //Defense Stats
            case StatType.MaxHealth: 
                value = playerStats.GetMaxHealth();
                break;
            case StatType.HealthRegen: 
                value = playerStats.resource.healthRegen.GetValue();
                break;
            case StatType.Evasion: 
                value = playerStats.GetEvasion();
                break;
            case StatType.Armor: 
                value = playerStats.GetBaseArmor();
                break;
            // Elemental Resistance
            case StatType.IceResistance:
                value = playerStats.GetElementalResistance(ElementType.Ice) * 100;
                break;
            case StatType.FireResistance:
                value = playerStats.GetElementalResistance(ElementType.Fire) * 100;
                break;
            case StatType.LightningResistance:
                value = playerStats.GetElementalResistance(ElementType.Lightning) * 100;
                break;
        }

        statValue.text = value + ui.PercentString(statType);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTooltip.ShowTooltip(true, rect, statType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltip.ShowTooltip(false, null);
    }
}
