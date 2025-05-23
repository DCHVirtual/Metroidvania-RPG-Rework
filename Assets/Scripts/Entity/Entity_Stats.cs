using System.Collections.Generic;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    //[SerializeField] bool useDefaultStats = true;
    [field: SerializeField] public StatSetup_SO defaultStats { get; private set; }
    [field: SerializeField] public Stats_Resource resource { get; private set; }
    [field: SerializeField] public Stats_Offense offense { get; private set; }
    [field: SerializeField] public Stats_Defense defense { get; private set; }
    [field: SerializeField] public Stats_Major major { get; private set; }

    #region Stat Constrains and Multipliers
    public float maxArmorReduction { get; private set; } = 1.00f;
    public float maxEvasion { get; private set; } = 80f;
    public float maxResistance { get; private set; } = 75f;
    public float armorMitigationFactor { get; private set; } = 100f;
    public float secondaryElementalDmgMult { get; private set; } = 0.5f;
    public float agilityEvasionMult { get; private set; } = 0.50f;
    public float agilityCritChanceMult { get; private set; } = 0.30f;
    public float strengthDamageMult { get; private set; } = 1.00f;
    public float strengthCritDamageMult { get; private set; } = 0.50f;
    public float vitalityHealthMult { get; private set; } = 5.00f;
    public float vitalityArmorMult { get; private set; } = 1.00f;
    public float intelliElemDmgMult { get; private set; } = 1.00f;
    public float intelliElemResistMult { get; private set; } = 0.50f;
    #endregion

    #region Stat Calculations
    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1f)
    {
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue() * strengthDamageMult;
        float damage = baseDamage + bonusDamage;

        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * agilityCritChanceMult;
        float critChance = baseCritChance + bonusCritChance;

        float baseCritDamage = offense.critDamage.GetValue();
        float bonusCritDamage = major.strength.GetValue() * strengthCritDamageMult;
        float critDamage =  (baseCritDamage + bonusCritDamage) / 100;

        isCrit = Random.Range(0, 100) < critChance;
        float finalDamage = isCrit ? damage * critDamage : damage;

        return finalDamage * scaleFactor;
    }

    //Highest stat applies 100% dmg and status effect, other stats apply 50% dmg
    public float GetElementalDamage(out ElementType highestElement, float scaleFactor = 1f)
    {

        List<float> elementDamages = new List<float>();
        elementDamages.Add(offense.fireDamage.GetValue());
        elementDamages.Add(offense.iceDamage.GetValue());
        elementDamages.Add(offense.lightningDamage.GetValue());

        float bonusElementalDamage = major.intelligence.GetValue() * intelliElemDmgMult;
        highestElement = GetHighestDamageElement(elementDamages);

        if (highestElement == ElementType.None) return 0f;

        float totalElementalDamage = bonusElementalDamage;

        for (int i = 0; i < elementDamages.Count; i++)
        {
            if (i == (int)highestElement)
                totalElementalDamage += elementDamages[i];
            else
                totalElementalDamage += elementDamages[i] * secondaryElementalDmgMult;
        }

        Debug.Log($"Elemental Damage: {totalElementalDamage} of type {highestElement}");
        return totalElementalDamage * scaleFactor;
    }

    public float GetArmorMitigation(float attackArmorReduction)
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue() * vitalityArmorMult;
        float armor = (baseArmor + bonusArmor);

        float reducedArmor = (baseArmor + bonusArmor) * (1 - attackArmorReduction);
        float mitigation = reducedArmor / (reducedArmor + armorMitigationFactor);

        return mitigation;
    }

    public float GetArmorReduction()
    {
        float reduction = offense.armorReduction.GetValue() / 100;
        return Mathf.Clamp(reduction, 0, maxArmorReduction);
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0f;
        float bonusResistance = major.intelligence.GetValue() * intelliElemResistMult;

        switch (element)
        {
            case ElementType.Fire:
                baseResistance = defense.fireResist.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defense.iceResist.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defense.lightningResist.GetValue();
                break;
        }

        float resistance = baseResistance + bonusResistance;

        return Mathf.Clamp(resistance, 0, maxResistance) / 100;
    }

    public float GetMaxHealth()
    {
        float baseHP = resource.maxHealth.GetValue();
        float bonusHP = major.vitality.GetValue() * vitalityHealthMult;
        return baseHP + bonusHP;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * agilityEvasionMult;
        float evasion = baseEvasion + bonusEvasion;

        return evasion < maxEvasion ? evasion : maxEvasion;
    }
    #endregion

    #region Helper Functions
    ElementType GetHighestDamageElement(List<float> elements)
    {
        float highestDamage = 0;
        ElementType type = ElementType.None;
        ElementType[] types = { ElementType.Fire, ElementType.Ice, ElementType.Lightning };

        for (int i = 0; i < types.Length; i++)
        {
            if (elements[i] > highestDamage)
            {
                highestDamage = elements[i];
                type = types[i];
            }
        }
        
        return type;
    }
    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return resource.maxHealth;
            case StatType.HealthRegen: return resource.healthRegen;

            case StatType.Strength: return major.strength;
            case StatType.Agility: return major.agility;
            case StatType.Intelligence: return major.intelligence;
            case StatType.Vitality: return major.vitality;

            case StatType.Damage: return offense.damage;
            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.CritChance: return offense.critChance;
            case StatType.CritDamage: return offense.critDamage;
            case StatType.ArmorReduction: return offense.armorReduction;

            case StatType.FireDamage: return offense.fireDamage;
            case StatType.IceDamage: return offense.iceDamage;
            case StatType.LightningDamage: return offense.lightningDamage;

            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;

            case StatType.IceResistance: return defense.iceResist;
            case StatType.FireResistance: return defense.fireResist;
            case StatType.LightningResistance: return defense.lightningResist;
            default: return null;
        }
    }

    [ContextMenu("Apply Default Stats")]
    public void ApplyDefaultStats()
    {
        //if (!useDefaultStats) return;

        resource.maxHealth.SetBaseValue(defaultStats.maxHealth);
        resource.healthRegen.SetBaseValue(defaultStats.healthRegen);

        major.strength.SetBaseValue(defaultStats.strength);
        major.agility.SetBaseValue(defaultStats.agility);
        major.intelligence.SetBaseValue(defaultStats.intelligence);
        major.vitality.SetBaseValue(defaultStats.vitality);

        offense.damage.SetBaseValue(defaultStats.damage);
        offense.attackSpeed.SetBaseValue(defaultStats.attackSpeed);
        offense.critChance.SetBaseValue(defaultStats.critChance);
        offense.critDamage.SetBaseValue(defaultStats.critDamage);
        offense.armorReduction.SetBaseValue(defaultStats.armorReduction);

        offense.fireDamage.SetBaseValue(defaultStats.fireDamage);
        offense.iceDamage.SetBaseValue(defaultStats.iceDamage);
        offense.lightningDamage.SetBaseValue(defaultStats.lightningDamage);

        defense.armor.SetBaseValue(defaultStats.armor);
        defense.evasion.SetBaseValue(defaultStats.evasion);

        defense.fireResist.SetBaseValue(defaultStats.fireResistance);
        defense.iceResist.SetBaseValue(defaultStats.iceResistance);
        defense.lightningResist.SetBaseValue(defaultStats.lightningResistance);
    }
    #endregion
}
