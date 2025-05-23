using System;
using UnityEngine;

[Serializable]
public class Stats_Offense
{
   
    //Physical Damage
    [field: SerializeField] public Stat damage { get; private set; }
    [field: SerializeField] public Stat attackSpeed { get; private set; }
    [field: SerializeField] public Stat critChance { get; private set; }
    [field: SerializeField] public Stat critDamage { get; private set; }
    [field: SerializeField] public Stat armorReduction { get; private set; }

    //Elemental Damage
    [field: SerializeField] public Stat fireDamage { get; private set; }
    [field: SerializeField] public Stat iceDamage { get; private set; }
    [field: SerializeField] public Stat lightningDamage { get; private set; }
}
