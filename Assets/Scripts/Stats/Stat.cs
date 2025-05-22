using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] protected float baseValue;

    public float GetValue()
    {
        return baseValue;
    }
}
