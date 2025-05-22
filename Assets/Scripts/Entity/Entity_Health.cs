using UnityEngine;
using UnityEngine.UI;
public class Entity_Health : MonoBehaviour, IDamageable
{
    [Header("Life Details")]
    [SerializeField] float currentHP;
    [SerializeField] public bool isDead;

    [Header("On Damage Knockback")]
    [SerializeField] protected float knockbackDuration = 0.2f;
    [SerializeField] protected Vector2 knockbackForce = new Vector2(3,2);

    protected EntityFX fx;
    Entity entity;
    Slider healthBar;
    Entity_Stats stats;

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        fx = GetComponent<EntityFX>();
        healthBar = GetComponentInChildren<Slider>();
        stats = GetComponent<Entity_Stats>();
        
    }

    protected virtual void Start()
    {
        currentHP = stats.GetMaxHP();
        UpdateHealthBar();
    }

    public virtual bool TakeDamage(float physicalDmg, Transform dmgDealer, 
        float elementalDmg, ElementType elemType)
    {
        if (isDead || AttackEvaded()) return false;

        var attackerStats = dmgDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        physicalDmg *= (1 - stats.GetArmorMitigation(armorReduction));
        elementalDmg *= (1 - stats.GetElementalResistance(elemType));

        float finalDamage = physicalDmg + elementalDmg;

        entity?.ReceiveKnockback(KnockbackDirectedForce(dmgDealer), knockbackDuration);

        ReduceHP(finalDamage);
        return true;
    }

    bool AttackEvaded() => Random.Range(0, 100) < stats.GetEvasion();

    public void ReduceHP(float dmg)
    {
        fx?.PlayDamageVFX();
        currentHP -= dmg;
        UpdateHealthBar();
        if (currentHP <= 0)
            Die();
    }

    public virtual void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    void UpdateHealthBar() => healthBar.value = currentHP / stats.GetMaxHP();
    

    Vector2 KnockbackDirectedForce(Transform dmgDealer)
    {
        int dir = transform.position.x > dmgDealer.position.x ? 1 : -1;
        return new Vector2(knockbackForce.x * dir, knockbackForce.y);
    }
}
