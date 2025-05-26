using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillObject_SwordBounce : SkillObject_Sword
{
    [SerializeField] float bounceSpeed = 25;
    [SerializeField] int bounceCount = 4;
    [SerializeField] bool isBouncing = false;

    [SerializeField] List<Collider2D> enemyTargets = new List<Collider2D>();

    protected override void Update()
    {
        GoToNextTarget();
        CheckReturnToPlayer();
    }

    public override void SetupSword(Skill_SwordThrow swordDetails, Vector2 forceDir)
    {
        base.SetupSword(swordDetails, forceDir);
        bounceCount = swordDetails.bounceCount;
        bounceSpeed = swordDetails.bounceSpeed;
        anim.SetBool("Spinning", true);
    }

    void GoToNextTarget()
    {
        if (CanBounce() && isBouncing)
        {
            var currentTarget = enemyTargets[0];

            if (currentTarget == null)
            {
                enemyTargets.RemoveAt(0);
                return;
            }

            transform.position = Vector2.MoveTowards(transform.position,
                currentTarget.transform.position, bounceSpeed * Time.deltaTime);

            CheckCollision(currentTarget);
        }
        else if (!CanBounce() && isBouncing)
            SetReturnToPlayer();
    }

    void CheckCollision(Collider2D currentTarget)
    {
        if (Vector2.Distance(transform.position, currentTarget.transform.position) < .1f)
        {
            DamageEnemy(currentTarget);
            bounceCount--;
            enemyTargets.RemoveAt(0);

            if (enemyTargets.Count == 0 && bounceCount > 0)
                enemyTargets = GetClosestEnemies();
        }
    }

    public bool CanBounce()
    {
        if (rb.simulated)
            return false;
        if (enemyTargets.Count == 0)
            return false;
        if (bounceCount == 0)
            return false;
        if (returnToPlayer)
            return false;
        return true;
    }

    List<Collider2D> GetClosestEnemies()
    {
        return GetEnemiesAround(transform, 15)
                .OrderBy(hit => Vector2.Distance(transform.position, hit.transform.position))
                .Skip(1).ToList();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            anim.SetBool("Spinning", false);
            transform.right = rb.linearVelocity;
            StopSword(collision);

            return;
        }

        enemyTargets = GetClosestEnemies();
        bounceCount--;
        rb.simulated = false;
        isBouncing = true;
        DamageEnemy(collision);
    }
}
