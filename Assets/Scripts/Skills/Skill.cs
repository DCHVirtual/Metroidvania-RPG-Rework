using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("General Details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;

    [SerializeField] protected float cooldown;
    protected float lastTimeUsed;

    protected virtual void Awake()
    {
        lastTimeUsed = lastTimeUsed - cooldown;
    }

    public void SetSkillUpgrade(SkillUpgradeType upgrade)
    {
        upgradeType = upgrade; 
    }

    public bool CanUseSkill()
    {
        if (OnCooldown())
            return false;

        return true;
    }

    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public void ReduceCooldown(float reduction) => lastTimeUsed -= reduction;

}
