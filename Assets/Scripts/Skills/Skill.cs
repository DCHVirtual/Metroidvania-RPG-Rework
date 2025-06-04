using UnityEngine;

public class Skill : MonoBehaviour
{
    public Player player {  get; private set; }
    public Player_SkillManager skillManager { get; private set; }

    public Data_DamageScale damageScale { get; private set; }

    [Header("General Details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;

    [field: SerializeField] public float cooldown { get; protected set; }
    public float lastTimeUsed { get; protected set; }

    protected virtual void Awake()
    {
        skillManager = GetComponentInParent<Player_SkillManager>();
        lastTimeUsed = lastTimeUsed - cooldown;
        player = GetComponentInParent<Player>();
        damageScale = new Data_DamageScale();
        ResetCooldown();
    }

    public virtual void TryUseSkill()
    {

    }

    public void SetSkillUpgrade(SkillData_SO skillData)
    {
        var upgrade = skillData.upgradeData;
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        damageScale = upgrade.damageScale;

        if (IsBaseSkill(skillData))
            player.ui.uiInGame.AssignSkillSlot(skillData);
    }

    public bool IsBaseSkill(SkillData_SO skillData)
    {
        return skillData.upgradeData.upgradeType == SkillUpgradeType.Dash ||
                skillData.upgradeData.upgradeType == SkillUpgradeType.TimeEcho ||
                skillData.upgradeData.upgradeType == SkillUpgradeType.Shard ||
                skillData.upgradeData.upgradeType == SkillUpgradeType.SwordThrow ||
                skillData.upgradeData.upgradeType == SkillUpgradeType.Domain_Slowdown;
                
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;

    public virtual bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None) 
            return false;

        if (OnCooldown())
            return false;

        return true;
    }

    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public void ReduceCooldown(float reduction) => lastTimeUsed -= reduction;
    public void ResetCooldown() => lastTimeUsed = Time.time - cooldown;
    public virtual void ResetSkill() => upgradeType = SkillUpgradeType.None;

}
