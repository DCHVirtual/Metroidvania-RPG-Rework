using UnityEngine;

public class Data_Attack
{
    public float physicalDamage;
    public float elementalDamage;
    public bool isCrit;
    public ElementType element;
    public Data_Elemental elementData;

    public Data_Attack(Entity_Stats stats, Data_DamageScale damageScale)
    {
        physicalDamage = stats.GetPhysicalDamage(out isCrit, damageScale.physical);
        elementalDamage = stats.GetElementalDamage(out element, damageScale.elemental);

        elementData = new Data_Elemental(stats, damageScale);
    }
}
