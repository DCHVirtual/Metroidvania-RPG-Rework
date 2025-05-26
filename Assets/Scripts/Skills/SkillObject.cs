using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SkillObject : MonoBehaviour, IAttackerTransform
{
    [SerializeField] GameObject onHitVfx;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;

    protected Animator anim;
    protected Entity_Stats stats;
    protected Data_DamageScale damageScale;
    protected ElementType usedElement;
    protected bool targetGotHit;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach (var target in EnemiesAround(t, radius))
            DamageEnemy(target);
    }

    protected void DamageEnemy(Collider2D target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable == null)
            return;

        Data_Attack attackData = stats.GetAttackData(damageScale);

        targetGotHit = damageable.TakeDamage(attackData.physicalDamage, GetAttackerTransform(),
            attackData.elementalDamage, attackData.element);

        if (attackData.element != ElementType.None)
            target.GetComponent<Entity_StatusHandler>()?
                .ApplyStatusEffect(attackData.element, attackData.elementData);

        if (targetGotHit)
            Instantiate(onHitVfx, target.transform.position, Quaternion.identity);

        usedElement = attackData.element;
    }

    public Transform ClosestTarget()
    {
        var closestEnemy = EnemiesAround(transform, 15)
            .Where(enemy => enemy.GetComponent<Enemy>() != null)
            .OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position))
            .FirstOrDefault();

        return closestEnemy.transform;
    }

    protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, enemyLayer);
    }

    protected virtual void OnDrawGizmos()
    {
        if (targetCheck == null)
            targetCheck = transform;

        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
    }

    public Transform GetAttackerTransform()
    {
        return Player.playerTransform;
    }
}
