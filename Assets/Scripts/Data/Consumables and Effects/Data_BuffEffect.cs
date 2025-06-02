using System;
using System.Xml;
using UnityEngine;

[Serializable]
public class Data_BuffEffect
{
    public Data_BuffEffect(Buff[] buffs, float duration, string source)
    {
        this.buffs = buffs;
        this.source = source;
        this.duration = duration;
        ID = Guid.NewGuid();
    }

    public void ApplyBuffs(Entity_Stats stats)
    {
        foreach (var buff in buffs)
            stats.GetStatByType(buff.type).AddModifier(buff.value, source, ID);
        
    }

    public void RemoveBuffs(Entity_Stats stats)
    {
        foreach (var buff in buffs)
            stats.GetStatByType(buff.type).RemoveModifier(ID);
    }


    [field: SerializeField] public Buff[] buffs { get; private set; }
    [field: SerializeField] public float duration { get; private set; }
    [field: SerializeField] public string source { get; private set; }
    public Guid ID { get; private set; }
}

[Serializable]
public class Buff
{
    public StatType type;
    public float value;
}
