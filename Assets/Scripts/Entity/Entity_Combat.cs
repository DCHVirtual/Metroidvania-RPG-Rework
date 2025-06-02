using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDamageDealt;

    [Header("Hit Detection")]
    [SerializeField] Transform hitCheck;
    [SerializeField] float hitCheckRadius;
    [SerializeField] LayerMask targetMask;

    EntityFX fx;
    Entity_Stats stats;
    public Data_DamageScale basicAttackScale;

    private void Awake()
    {
        fx = GetComponent<EntityFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack(/*Eventually write functionality to pass attack data here depending on attack*/)
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null)
                continue;
            

            Data_Attack attackData = stats.GetAttackData(basicAttackScale);

            bool targetGotHit = damageable.TakeDamage(attackData.physicalDamage, transform, attackData.elementalDamage, attackData.element);
            var vfxPos = (Vector2)target.transform.position + Random.insideUnitCircle * .5f;

            if (targetGotHit)
            {
                OnDamageDealt?.Invoke(attackData.physicalDamage);
                fx.PlayHitVFX(vfxPos, attackData.isCrit, attackData.element);
                if (attackData.element != ElementType.None)
                    target.GetComponent<Entity_StatusHandler>()?.ApplyStatusEffect(attackData.element, attackData.elementData);
            }
        }
    }

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(hitCheck.position, hitCheckRadius, targetMask);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hitCheck.position, hitCheckRadius);
    }
}
