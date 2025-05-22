
using System.Collections;
using UnityEngine;
using Color = UnityEngine.Color;

public class Entity_StatusHandler : MonoBehaviour
{
    [SerializeField] ElementType currentStatus = ElementType.None;
    [SerializeField] GameObject burnVFX;
    [SerializeField] GameObject lightningVFX;
    Entity entity;
    Entity_Health entityHealth;
    Entity_Stats entityStats;
    EntityFX entityFX;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityStats = GetComponent<Entity_Stats>();
        entityHealth = GetComponent<Entity_Health>();
        entityFX = GetComponent<EntityFX>();
    }

    public void ApplyChilledStatus(float duration, float speedMultiplier)
    {
        duration *= (1 - entityStats.GetElementalResistance(ElementType.Ice));
        StartCoroutine(ApplyChillCo(duration, speedMultiplier));

    }
    public void ApplyBurnedStatus(float duration, float fireDamage)
    {
        fireDamage *= (1 - entityStats.GetElementalResistance(ElementType.Fire));
        StartCoroutine(ApplyBurnCo(duration, fireDamage));
    }

    public void ApplyElectrifiedStatus()
    {
        Instantiate(lightningVFX, transform);
    }

    IEnumerator ApplyChillCo(float duration, float speedMultiplier)
    {
        entity.ChillEntity(duration, speedMultiplier);
        currentStatus = ElementType.Ice;
        entityFX.PlayStatusVFX(duration, currentStatus);
        yield return new WaitForSeconds(duration);
        currentStatus = ElementType.None;
    }

    IEnumerator ApplyBurnCo(float duration, float totalDamage)
    {
        currentStatus = ElementType.Fire;
        entityFX.PlayStatusVFX(duration, currentStatus);

        int ticksPerSec = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSec * duration);
        float dmgPerTick = totalDamage / tickCount;
        float tickInterval = 1f / ticksPerSec;

        for (int i = 0; i < tickCount; i++)
        {
            entityHealth.ReduceHP(dmgPerTick);
            Instantiate(burnVFX, transform);
            yield return new WaitForSeconds(tickInterval);
        }

        currentStatus = ElementType.None;
    }

    public bool CanBeApplied(ElementType element) => currentStatus == ElementType.None;
    
}
