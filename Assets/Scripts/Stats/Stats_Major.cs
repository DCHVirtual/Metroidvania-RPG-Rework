using System;
using UnityEngine;

[Serializable]
public class Stats_Major
{
    [field: SerializeField] public Stat strength { get; private set; }
    [field: SerializeField] public Stat agility { get; private set; }
    [field: SerializeField] public Stat intelligence { get; private set; }
    [field: SerializeField] public Stat vitality { get; private set; }
}
