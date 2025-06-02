using UnityEngine;

public class Skill : MonoBehaviour
{
    public Player player {  get; private set; }
    public Player_SkillManager skillManager { get; private set; }

    public Data_DamageScale damageScale { get; private set; }

    [Header("General Details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;

    [SerializeField] protected float cooldown;
    protected float lastTimeUsed;

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

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        damageScale = upgrade.damageScale;
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
