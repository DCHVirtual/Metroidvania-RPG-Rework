using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    [Header("Hit Detection")]
    [SerializeField] Transform hitCheck;
    [SerializeField] float hitCheckRadius;
    [SerializeField] LayerMask targetMask;
    EntityFX fx;
    Entity entity;

    private void Awake()
    {
        fx = GetComponent<EntityFX>();
    }

    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            var damageable = target.GetComponent<IDamageable>();

            if (damageable == null)
                continue;

            damageable.TakeDamage(10, transform);
            fx.PlayHitVFX((Vector2)target.transform.position + Random.insideUnitCircle * .5f);
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
