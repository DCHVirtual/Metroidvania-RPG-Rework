using System;
using UnityEngine;

[Serializable]
public class Stats_Resource
{
    [field: SerializeField] public Stat maxHealth { get; private set; }
    [field: SerializeField] public Stat healthRegen { get; private set; }
}