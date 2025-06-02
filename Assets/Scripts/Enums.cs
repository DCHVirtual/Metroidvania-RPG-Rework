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
    LightningResistance,
    ElementalDamage
}

public enum SkillType
{
    Dash,
    TimeEcho,
    TimeShard,
    SwordThrow,
    Domain
}

public enum SkillUpgradeType
{
    None,

    Dash,
    Dash_CloneOnStart,
    Dash_CloneOnStartAndArrival,
    Dash_ShardOnStart,
    Dash_ShardOnStartAndArrival,

    Shard,
    Shard_MoveToEnemy,
    Shard_MultiCast,
    Shard_Teleport,
    Shard_TeleportHpRewind,

    SwordThrow,
    SwordThrow_Spin,
    SwordThrow_Pierce,
    SwordThrow_Bounce,

    TimeEcho,
    TimeEcho_SingleAttack,
    TimeEcho_MultiAttack,
    TimeEcho_Multiply,
    TimeEcho_HealingWisp,
    TimeEcho_CleanseWisp,
    TimeEcho_CooldownWisp,

    Domain_Slowdown,
    Domain_EchoSpam,
    Domain_ShardSpam
}

public enum ItemType
{
    Material,
    Weapon,
    Armor,
    Trinket,
    Consumable
}

public enum ItemRarity
{
    Common,
    Rare,
    Legendary
}