using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill Data - ")]
public class SkillData_SO : ScriptableObject
{
    public string displayName;
    public int cost;
    public SkillType skillType;
    public bool unlockedByDefault;
    public UpgradeData upgradeData;
    [TextArea]
    public string description;
    public Sprite icon;
}

[Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgradeType;
    public float cooldown;
    public Data_DamageScale damageScale;
}
