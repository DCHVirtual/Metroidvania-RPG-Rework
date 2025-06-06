using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Entity_Health : MonoBehaviour, IDamageable
{
    public event Action OnTakingDamage;
    public event Action OnHealthBarUpdate;

    [Header("Life Details")]
    [SerializeField] float currentHealth;
    [HideInInspector] public bool isDead;

    [Header("On Damage Knockback")]
    [SerializeField] protected float knockbackDuration = 0.2f;
    [SerializeField] protected Vector2 knockbackForce = new Vector2(3,2);

    [Header("Health Regen")]
    [SerializeField] float regenInterval = 1f;
    [SerializeField] bool canRegenHealth = true;

    protected EntityFX fx;
    Entity entity;
    Slider healthBar;
    Entity_Stats stats;
    public float damageTaken { get; private set; }

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        fx = GetComponent<EntityFX>();
        healthBar = GetComponentInChildren<Slider>();
        stats = GetComponent<Entity_Stats>();
        if (stats == null)
            stats = GetComponent<Player_Stats>();
        
    }

    protected virtual void Start()
    {
        if (stats != null)
        {
            currentHealth = stats.GetMaxHealth();
            UpdateHealthBar();
            InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
        }
    }

    public virtual bool TakeDamage(float physicalDmg, Transform dmgDealer, 
        float elementalDmg, ElementType elemType)
    {
        if (isDead || AttackEvaded()) return false;

        var attackerStats = dmgDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        if (stats != null)
        {
            physicalDmg *= (1 - stats.GetArmorMitigation(armorReduction));
            elementalDmg *= (1 - stats.GetElementalResistance(elemType));
        }

        float finalDamage = physicalDmg + elementalDmg;

        entity?.ReceiveKnockback(KnockbackDirectedForce(dmgDealer), knockbackDuration);

        ReduceHealth(finalDamage);

        OnTakingDamage?.Invoke();

        return true;
    }

    void RegenerateHealth()
    {
        if (!canRegenHealth) return;

        Heal(stats.resource.healthRegen.GetValue());
    }

    public void ReduceHealth(float damage)
    {
        if (isDead) return;

        fx?.PlayDamageVFX();
        currentHealth -= damage;
        damageTaken += damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        float newHealth = currentHealth + amount;
        float maxHealth = stats.GetMaxHealth();
        currentHealth = Mathf.Min(newHealth, maxHealth);
        UpdateHealthBar();
    }


    protected virtual void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    public float GetHealthPercent()
    {
        if (stats ==  null) return 0;

        return currentHealth / stats.GetMaxHealth();
    }
        public void SetHealthPercent(float percent)
    { 
        currentHealth = Mathf.Clamp01(percent) * stats.GetMaxHealth();
        UpdateHealthBar();
    }

    public float GetCurrentHealth() => currentHealth;

    void UpdateHealthBar() 
    {
        if (stats == null) return;
        if (!GetComponent<Player>())
            healthBar.value = currentHealth / stats.GetMaxHealth(); 
        OnHealthBarUpdate?.Invoke();
    }

    bool AttackEvaded()
    {
        if (stats == null) return false;
        return Random.Range(0, 100) < stats.GetEvasion();
    }

    Vector2 KnockbackDirectedForce(Transform dmgDealer)
    {
        int dir = transform.position.x > dmgDealer.position.x ? 1 : -1;
        return new Vector2(knockbackForce.x * dir, knockbackForce.y);
    }
}
