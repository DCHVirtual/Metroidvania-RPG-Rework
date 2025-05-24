using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash { get; private set; }

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
    }

    public Skill GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash: return dash;
            default: return null;
        }
    }
}
