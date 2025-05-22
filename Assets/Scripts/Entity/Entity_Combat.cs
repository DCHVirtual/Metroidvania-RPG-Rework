using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    [Header("Hit Detection")]
    [SerializeField] Transform hitCheck;
    [SerializeField] float hitCheckRadius;
    [SerializeField] LayerMask targetMask;

    [Header("Status Effect Details")]
    [SerializeField] float statusDuration = 3f;
    [SerializeField] float chillSlowMultiplier = 0.4f;
    EntityFX fx;
    Entity_Stats stats;

    private void Awake()
    {
        fx = GetComponent<EntityFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            var damageable = target.GetComponent<IDamageable>();
            if (damageable == null)
                continue;

            float physicalDamage = stats.GetPhysicalDamage(out bool isCrit);
            float elementalDamage = stats.GetElementalDamage(out ElementType elemType);

            bool damaged = damageable.TakeDamage(physicalDamage, transform, elementalDamage, elemType);
            var vfxPos = (Vector2)target.transform.position + Random.insideUnitCircle * .5f;

            if (damaged)
            {
                fx.PlayHitVFX(vfxPos, isCrit, elemType);
                if (elemType != ElementType.None)
                    ApplyStatusEffect(target.transform, elemType);
            }
        }
    }

    public void ApplyStatusEffect(Transform target, ElementType element, float scaleFactor = 1f)
    {
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

        if (statusHandler == null) return;

        if (element == ElementType.Ice && statusHandler.CanBeApplied(ElementType.Ice))
            statusHandler.ApplyChilledStatus(statusDuration, chillSlowMultiplier);
        else if (element == ElementType.Fire && statusHandler.CanBeApplied(ElementType.Fire))
        {
            float fireDamage = stats.offense.fireDamage.GetValue() * scaleFactor;
            statusHandler.ApplyBurnedStatus(statusDuration, fireDamage);
        }
        else if (element == ElementType.Lightning && statusHandler.CanBeApplied(ElementType.Lightning))
            statusHandler.ApplyElectrifiedStatus(/*statusDuration, chillSlowMultiplier*/);
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
