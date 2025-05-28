using UnityEngine;

public class SkillObject_SwordSpin : SkillObject_Sword
{
    float attacksPerSecond;
    float attackTimer;
    float autoReturnTimer;

    public override void SetupSword(Skill_SwordThrow swordDetails, Vector2 forceDir)
    {
        base.SetupSword(swordDetails, forceDir);

        attacksPerSecond = swordDetails.attacksPerSecond;
        autoReturnTimer = swordDetails.autoReturnTimer;
        anim.SetBool("Spinning", true);
    }

    protected override void Update()
    {
        HandleAutoReturn();
        HandleAttack();
        CheckReturnToPlayer();
    }

    void HandleAutoReturn()
    {
        if (rb.simulated)
            return;

        autoReturnTimer -= Time.deltaTime;

        if (autoReturnTimer < 0)
            SetReturnToPlayer();
    }

    void HandleAttack()
    {
        if (rb.simulated)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer < 0)
        {
            DamageEnemiesInRadius(transform, 1);
            attackTimer = 1 / attacksPerSecond;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        rb.simulated = false;
        transform.localScale *= 1.3f;
    }
}