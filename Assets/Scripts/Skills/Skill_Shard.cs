using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill
{
    [SerializeField] GameObject shardPrefab;
    [SerializeField] float detonationTime = 4.5f;

    [Header("Moving Shard Upgrade")]
    [SerializeField] float shardSpeed = 7.5f;

    [Header("Mulitcast Shard Upgrade")]
    [SerializeField] int maxCharges = 3;
    [SerializeField] int currentCharges = 3;
    [SerializeField] bool isRecharging;

    [Header("Teleport Shard Upgrade")]
    [SerializeField] float teleportDetonationTime = 10;

    [Header("Health Rewind Shard Upgrade")]
    [SerializeField] float savedHealthPercent;

    SkillObject_Shard currentShard;
    Entity_Health playerHealth;


    private void Start()
    {
        currentCharges = maxCharges;
        playerHealth = GetComponentInParent<Entity_Health>();
    }

    public override void TryUseSkill()
    {
        if (!CanUseSkill())
            return;

        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();
        else if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();
        else if (Unlocked(SkillUpgradeType.Shard_MultiCast))
            HandleShardMulticast();
        else if (Unlocked(SkillUpgradeType.Shard_Teleport))
            HandleShardTeleport();
        else if (Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            HandleShardHealthRewind();
    }

    void HandleShardTeleport()
    {
        if (currentShard == null)
            CreateShard();
        else
        {
            SwapPlayerAndShard();
            currentShard.Explode();
            SetSkillOnCooldown();
        }
    }
    void HandleShardHealthRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthPercent = playerHealth.GetHealthPercent();
        }
        else
        {
            SwapPlayerAndShard();
            currentShard.Explode();
            playerHealth.SetHealthPercent(savedHealthPercent);
            SetSkillOnCooldown();
        }
    }

    void SwapPlayerAndShard()
    {
        var shardPos = currentShard.transform.position;
        var playerPos = player.transform.position;

        currentShard.transform.position = playerPos;
        player.TeleportPlayer(shardPos);
    }

    void HandleShardMulticast()
    {
        if (currentCharges == 0)
            return;

        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharges--;

        if (isRecharging == false)
            StartCoroutine(ShardRechargeCo());
    }

    IEnumerator ShardRechargeCo()
    {
        isRecharging = true;

        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            currentCharges++;
        }

        isRecharging = false;
    }

    void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        SetSkillOnCooldown();
    }

    public void CreateShard()
    {
        var shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(this);

        if (Unlocked(SkillUpgradeType.Shard_Teleport) ||
            Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            currentShard.OnShardExplode += ForceCooldown;
    }

    public void CreateRawShard()
    {
        bool canMove = Unlocked(SkillUpgradeType.Shard_MoveToEnemy) ||
            Unlocked(SkillUpgradeType.Shard_MultiCast);

        var shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        shard.GetComponent<SkillObject_Shard>().SetupShard(this, detonationTime, canMove, shardSpeed);
    }

    void ForceCooldown()
    {
        if (!OnCooldown())
        {
            SetSkillOnCooldown();
            currentShard.OnShardExplode -= ForceCooldown;
        }
    }

    public float GetDetonationTime()
    {
        if (Unlocked(SkillUpgradeType.Shard_Teleport) ||
            Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            return teleportDetonationTime;

        else return detonationTime;
    }
}
