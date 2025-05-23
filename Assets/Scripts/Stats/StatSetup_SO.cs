using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Stat Setup", fileName = "Default Stats")]
public class StatSetup_SO : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth = 100;
    public float healthRegen = 1;

    [Header("Offense - Physical")]
    public float attackSpeed = 1;
    public float damage = 10;
    public float critChance = 0;
    public float critDamage = 150;
    public float armorReduction = 0;

    [Header("Offense - Elemental")]
    public float fireDamage = 0;
    public float iceDamage = 0;
    public float lightningDamage = 0;

    [Header("Defense")]
    public float armor = 0;
    public float evasion = 0;
    public float fireResistance = 0;
    public float iceResistance = 0;
    public float lightningResistance = 0;

    [Header("Major (Player only)")]
    public float strength = 0;
    public float agility = 0;
    public float intelligence = 0;
    public float vitality = 0;
}
