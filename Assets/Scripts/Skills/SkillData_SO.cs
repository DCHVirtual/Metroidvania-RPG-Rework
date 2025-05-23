using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill Data - ")]
public class SkillData_SO : ScriptableObject
{
    public int cost;
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;
}
