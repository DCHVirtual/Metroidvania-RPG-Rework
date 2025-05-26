using System;
using UnityEngine;

[Serializable]
public class Data_Elemental
{
    public float chillDuration;
    public float chillSlowMultiplier;

    public float burnDuration;
    public float burnDamage;

    public float shockDuration;
    public float shockDamage;
    public float shockCharge;

    public Data_Elemental(Entity_Stats stats, Data_DamageScale damageScale)
    {
        chillDuration = damageScale.chillDuration;
        chillSlowMultiplier = damageScale.chillSlowMultiplier;

        burnDuration = damageScale.burnDuration;
        burnDamage = stats.offense.fireDamage.GetValue() * damageScale.burnDamageScale;
        
        shockDuration = damageScale.shockDuration;
        shockDamage = stats.offense.lightningDamage.GetValue() * damageScale.shockDamageScale;
        shockCharge = damageScale.shockCharge;
    }
}
