using System;
using UnityEngine;

public class SkillObject_Shard : SkillObject
{
    public event Action OnShardExplode;
    Skill_Shard shardDetails;

    [SerializeField] GameObject explodePrefab;
    Transform target;
    float speed;

    private void Update()
    {
        if (target == null)
            return;

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void MoveTowardsClosestTarget(float speed)
    {
        target = ClosestTarget();
        this.speed = speed;
    }

    public void SetupShard(Skill_Shard shardDetails)
    {
        this.shardDetails = shardDetails;
        stats = shardDetails.player.stats;
        damageScale = shardDetails.damageScale;

        Invoke(nameof(Explode), shardDetails.GetDetonationTime());
    }

    public void SetupShard(Skill_Shard shardDetails, float detonationTime, bool canMove, float shardSpeed)
    {
        this.shardDetails = shardDetails;
        stats = shardDetails.player.stats;
        damageScale = shardDetails.damageScale;

        Invoke(nameof(Explode), detonationTime);

        if (canMove)
            MoveTowardsClosestTarget(shardSpeed);
    }

    public void Explode()
    {
        DamageEnemiesInRadius(transform, checkRadius);
        var vfx = Instantiate(explodePrefab, transform.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = 
            shardDetails.player.fx.ElementHitVFXColor(usedElement);
        OnShardExplode?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
            return;

        Explode();
    }
}
