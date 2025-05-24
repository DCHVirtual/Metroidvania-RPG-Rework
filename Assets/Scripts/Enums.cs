using UnityEngine;

public enum ElementType
{
    Fire,
    Ice,
    Lightning,
    None
}

public enum StatType
{
    MaxHealth,
    HealthRegen,
    Strength,
    Agility,
    Intelligence,
    Vitality,
    Damage,
    AttackSpeed,
    CritChance,
    CritDamage,
    ArmorReduction,
    FireDamage,
    IceDamage,
    LightningDamage,
    Armor,
    Evasion,
    IceResistance,
    FireResistance,
    LightningResistance
}

public enum SkillType
{
    Dash,
    TimeEcho
}

public enum SkillUpgradeType
{
    Dash,
    Dash_CloneOnStart,
    Dash_CloneOnStartAndArrival,
    Dash_ShardOnStart,
    Dash_ShardOnStartAndArrival
}