using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill Data - ")]
public class SkillData_SO : ScriptableObject
{
    public string displayName;
    public int cost;
    public SkillType skillType;
    public SkillUpgradeType upgradeType;
    [TextArea]
    public string description;
    public Sprite icon;
}
