using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] protected float baseValue;
    [SerializeField] protected List<StatModifier> modifiers = new List<StatModifier>();
    bool needToCalculate = true;
    float finalValue;

    public float GetValue()
    {
        return GetFinalValue();
    }

    public void AddModifier(float value, string source, Guid ID)
    {
        modifiers.Add(new StatModifier(value, source, ID));
        needToCalculate = true;
    }

    public void RemoveModifier(Guid ID)
    {
        modifiers.RemoveAll(modifier => modifier.ID.Equals(ID));
        needToCalculate = true;
    }

    float GetFinalValue()
    {
        if (needToCalculate)
        {
            float finalVal = baseValue;
            foreach (var modifier in modifiers)
                finalVal += modifier.value;
            needToCalculate = false;
            finalValue = finalVal;
        }

        return finalValue;
    }

    public void SetBaseValue(float value) => baseValue = value;
}

[Serializable]
public class StatModifier
{
    public float value { get; private set; }
    public string source { get; private set; }
    public Guid ID { get; private set; }

    public StatModifier(float value, string source, Guid ID)
    {
        this.value = value;
        this.source = source;
        this.ID = ID;
    }
}
