using System;
using UnityEngine;

[Serializable]
public class Stats_Defense
{
    //Physical Defense
    [field: SerializeField] public Stat armor { get; private set; }
    [field: SerializeField] public Stat evasion { get; private set; }
    
    //Elemental Resistance
    [field: SerializeField] public Stat fireResist { get; private set; }
    [field: SerializeField] public Stat iceResist { get; private set; }
    [field: SerializeField] public Stat lightningResist { get; private set; }
}
