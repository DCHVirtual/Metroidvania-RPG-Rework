using Unity.VisualScripting;
using UnityEngine;

public class Skill_Dash : Skill
{

    public void OnStartEffect()
    {
        if (Unlocked(SkillUpgradeType.Dash_CloneOnStart) ||
            Unlocked(SkillUpgradeType.Dash_CloneOnStartAndArrival))
            CreateClone();
        if (Unlocked(SkillUpgradeType.Dash_ShardOnStart) ||
            Unlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival))
            CreateShard();
    }
    
    public void OnEndEffect()
    {
        if (Unlocked(SkillUpgradeType.Dash_CloneOnStartAndArrival))
            CreateClone();
        if (Unlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival))
            CreateShard();
    }
    void CreateShard()
    {
        skillManager.shard.CreateRawShard();
    }

    void CreateClone()
    {
        skillManager.timeEcho.CreateTimeEcho();
    }

}
