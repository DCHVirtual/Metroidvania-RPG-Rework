using UnityEngine;

public class SkillObject_Sword : SkillObject
{
    protected Skill_SwordThrow swordDetails;

    protected Transform playerTransform;
    protected bool returnToPlayer;
    protected bool isStuck = false;
    protected float returnSpeed = 25f;
    protected float maxAllowedDistance = 25;


    protected virtual void Update()
    {
        if (!isStuck)
            transform.right = rb.linearVelocity;

        CheckReturnToPlayer();
    }

    protected void CheckReturnToPlayer()
    {
        float distToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distToPlayer > maxAllowedDistance)
            SetReturnToPlayer();

        if (returnToPlayer == false) return;

        transform.position = Vector2.MoveTowards(transform.position,
                playerTransform.position, returnSpeed * Time.deltaTime);

        if (distToPlayer < .5f)
            Destroy(gameObject);
    }

    public void SetReturnToPlayer()
    {
        anim.SetBool("Spinning", true);
        transform.localScale = Vector3.one;
        returnToPlayer = true;
    }

    public virtual void SetupSword(Skill_SwordThrow swordDetails, Vector2 forceDir)
    {
        rb.linearVelocity = forceDir;
        this.swordDetails = swordDetails;
        stats = swordDetails.player.stats;
        damageScale = swordDetails.damageScale;
        playerTransform = swordDetails.player.transform;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StopSword(collision);
        DamageEnemy(collision);
    }

    protected void StopSword(Collider2D collision)
    {
        isStuck = true;
        transform.parent = collision.transform;
        rb.simulated = false;
    }
}
