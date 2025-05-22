using UnityEngine;

public interface IDamageable
{
    public bool TakeDamage(float dmg, Transform dmgDealer, 
        float elementalDmg = 0f, ElementType elemType = ElementType.None);
}
