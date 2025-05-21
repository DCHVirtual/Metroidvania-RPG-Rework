using UnityEngine;

public class Entity_Health : MonoBehaviour, IDamageable
{
    [Header("Life Details")]
    [SerializeField] float maxHP = 100f;
    [SerializeField] float currentHP;
    [SerializeField] public bool isDead;

    [Header("On Damage Knockback")]
    [SerializeField] protected float knockbackDuration = 0.2f;
    [SerializeField] protected Vector2 knockbackForce = new Vector2(3,2);

    protected EntityFX fx;
    Entity entity;

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        fx = GetComponent<EntityFX>();
        currentHP = maxHP;
    }

    public virtual void TakeDamage(float dmg, Transform dmgDealer)
    {
        if (isDead)
            return;

        fx?.PlayDamageVFX();
        entity?.ReceiveKnockback(KnockbackDirectedForce(dmgDealer), knockbackDuration);

        ReduceHP(dmg);
    }

    protected void ReduceHP(float dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
            Die();
    }

    public virtual void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    Vector2 KnockbackDirectedForce(Transform dmgDealer)
    {
        int dir = transform.position.x > dmgDealer.position.x ? 1 : -1;
        return new Vector2(knockbackForce.x * dir, knockbackForce.y);
    }
}
