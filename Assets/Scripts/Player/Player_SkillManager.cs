using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash { get; private set; }
    public Skill_Shard shard { get; private set; }
    public Skill_SwordThrow swordThrow { get; private set; }
    public Skill_TimeEcho timeEcho { get; private set; }
    public Skill_Domain domain { get; private set; }

    Skill[] allSkills;

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
        shard = GetComponentInChildren<Skill_Shard>();
        swordThrow = GetComponentInChildren<Skill_SwordThrow>();
        timeEcho = GetComponentInChildren<Skill_TimeEcho>();
        domain = GetComponentInChildren<Skill_Domain>();

        allSkills = GetComponentsInChildren<Skill>();
    }

    public void ReduceAllCooldownsBy(float seconds)
    {
        foreach (var skill in allSkills)
            skill.ReduceCooldown(seconds);
    }

    public Skill GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash: return dash;
            case SkillType.TimeShard: return shard;
            case SkillType.SwordThrow: return swordThrow;
            case SkillType.TimeEcho: return timeEcho;
            case SkillType.Domain: return domain;
            default: return null;
        }
    }
}
