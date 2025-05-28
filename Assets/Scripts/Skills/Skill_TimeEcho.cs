using UnityEngine;

public class Skill_TimeEcho : Skill
{
    [SerializeField] GameObject timeEchoPrefab;
    [SerializeField] float timeEchoDuration;

    [Header("Attack Upgrades")]
    [SerializeField] float multiplyChance = .3f;
    int maxAttacks = 3;

    [Header("Healing Wisp Upgrades")]
    [SerializeField] float damagePercentHealed = .3f;
    [SerializeField] float cooldownSecondsReduced;

    public float GetPercentDamageHealed()
    {
        if (ShouldBeWisp())
            return damagePercentHealed;

        return 0;
    }

    public float GetCooldownReduction()
    {
        if (upgradeType != SkillUpgradeType.TimeEcho_CooldownWisp)
            return 0;

        return cooldownSecondsReduced;
    }

    public bool ShouldRemoveNegativeEffects()
    {
        return upgradeType == SkillUpgradeType.TimeEcho_CleanseWisp;
    }

    public override void TryUseSkill()
    {
        if (!CanUseSkill()) 
            return;

        CreateTimeEcho();
    }

    public void CreateTimeEcho(Vector3? position = null)
    {
        var newPosition = position ?? transform.position;
        var timeEcho = Instantiate(timeEchoPrefab, newPosition, Quaternion.identity);
        timeEcho.GetComponent<SkillObject_TimeEcho>().SetupEcho(this, GetMaxAttacks(), GetMultiplyChance());
    }

    float GetMultiplyChance()
    {
        return Unlocked(SkillUpgradeType.TimeEcho_Multiply) ? multiplyChance : 0;
    }

    int GetMaxAttacks()
    {
        if (Unlocked(SkillUpgradeType.TimeEcho_SingleAttack) || Unlocked(SkillUpgradeType.TimeEcho_Multiply))
            return 1;

        if (Unlocked(SkillUpgradeType.TimeEcho_MultiAttack))
            return maxAttacks;

        return 0;
    }

    public float GetEchoDuration()
    {
        return timeEchoDuration;
    }

    public bool ShouldBeWisp()
    {
        return Unlocked(SkillUpgradeType.TimeEcho_CooldownWisp) ||
             Unlocked(SkillUpgradeType.TimeEcho_HealingWisp) ||
             Unlocked(SkillUpgradeType.TimeEcho_CleanseWisp);
    }
}
