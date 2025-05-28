using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Domain : Skill
{
    [SerializeField] GameObject domainPrefab;

    [Header("Domain details")]
    public float maxDomainSize = 15;
    public float expandSpeed = 3;

    [Header("Slowdown Upgrade")]
    [SerializeField] float slowDownPercent = .8f;
    [SerializeField] float slowDownDomainDuration = 5;
    float spellCastTimer;
    float spellsPerSecond;

    [Header("Spell Casting Upgrade")]
    [SerializeField] int spellsToCast = 15;
    [SerializeField] float spellSpamSlowDown = 1;
    [SerializeField] float spellSpamDuration = 8;
    List<Enemy> trappedTargets = new List<Enemy>();
    Enemy currentTarget;

    Enemy FindNextTarget()
    {
        if (trappedTargets.Count == 0)
            return null;

        int randomIndex = Random.Range(0, trappedTargets.Count);
        Enemy target = trappedTargets[randomIndex];

        if (target == null || target.health.isDead)
        {
            trappedTargets.RemoveAt(randomIndex);
            return null;
        }

        return target;
    }

    public void CastSpells()
    {
        spellCastTimer -= Time.deltaTime;

        if (currentTarget == null)
            currentTarget = FindNextTarget();

        if (currentTarget != null && spellCastTimer < 0)
        {
            CastSpell(currentTarget.transform);
            spellCastTimer = 1 / spellsPerSecond;
            currentTarget = null;
        }
    }

    void CastSpell(Transform target)
    {
        if (upgradeType == SkillUpgradeType.Domain_EchoSpam)
        {
            Vector3 offset = new Vector2((Random.value < .5f? 1 : -1), 0);

            skillManager.timeEcho.CreateTimeEcho(target.position + offset);
        }
        else if (upgradeType == SkillUpgradeType.Domain_ShardSpam)
        {
            skillManager.shard.CreateRawShard(target, true);
        }
    }

    public bool InstantDomain()
    {
        return upgradeType != SkillUpgradeType.Domain_EchoSpam &&
            upgradeType != SkillUpgradeType.Domain_ShardSpam;
    }
    public void CreateDomain()
    {
        spellsPerSecond = spellsToCast / GetDuration();

        var domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_Domain>().SetupDomain(this);
    }

    public float GetDuration()
    {
        if (upgradeType == SkillUpgradeType.Domain_Slowdown)
            return slowDownDomainDuration;
        else
            return spellSpamDuration;
    }

    public float GetSlowPercent()
    {
        if (upgradeType == SkillUpgradeType.Domain_Slowdown)
            return slowDownPercent;
        else
            return spellSpamSlowDown;
    }

    public void AddTarget(Enemy enemy)
    {
        trappedTargets.Add(enemy);
    }

    public void ClearTargets()
    {
        trappedTargets.Clear();
    }
}
