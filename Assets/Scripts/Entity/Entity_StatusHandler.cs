
using System.Collections;
using UnityEngine;
using Color = UnityEngine.Color;

public class Entity_StatusHandler : MonoBehaviour
{
    ElementType currentStatus = ElementType.None;
    [SerializeField] GameObject burnVFX;
    [SerializeField] GameObject lightningVFX;
    Entity entity;
    Entity_Health entityHealth;
    Entity_Stats entityStats;
    EntityFX entityFX;

    [SerializeField] float currentCharge;
    [SerializeField] float maximumCharge;
    Coroutine shockCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityStats = GetComponent<Entity_Stats>();
        entityHealth = GetComponent<Entity_Health>();
        entityFX = GetComponent<EntityFX>();
    }

    public void ApplyStatusEffect(ElementType element, Data_Elemental elementData)
    {
        if (element == ElementType.Ice && CanBeApplied(ElementType.Ice))
            ApplyChillStatus(elementData.chillDuration, elementData.chillSlowMultiplier);
        else if (element == ElementType.Fire && CanBeApplied(ElementType.Fire))
            ApplyBurnStatus(elementData.burnDuration, elementData.burnDamage);
        else if (element == ElementType.Lightning && CanBeApplied(ElementType.Lightning))
            ApplyShockStatus(elementData.shockDuration, elementData.shockDamage, elementData.shockCharge);
    }

    public void ApplyChillStatus(float duration, float speedMultiplier)
    {
        duration *= (1 - entityStats.GetElementalResistance(ElementType.Ice));
        StartCoroutine(ApplyChillCo(duration, speedMultiplier));

    }
    public void ApplyBurnStatus(float duration, float damage)
    {
        damage *= (1 - entityStats.GetElementalResistance(ElementType.Fire));
        StartCoroutine(ApplyBurnCo(duration, damage));
    }

    public void ApplyShockStatus(float duration, float damage, float charge)
    {
        charge *= (1 - entityStats.GetElementalResistance(ElementType.Lightning));
        currentCharge += charge;

        if (currentCharge >= maximumCharge)
        {
            Instantiate(lightningVFX, transform);
            entityHealth.ReduceHealth(damage);
            StopShockEffect();
            return;
        }

        if (shockCo != null)
            StopCoroutine(shockCo);

        shockCo = StartCoroutine(ApplyShockCo(duration));
    }

    void StopShockEffect()
    {
        currentCharge = 0;
        currentStatus = ElementType.None;
        entityFX.StopAllVFX();
    }

    IEnumerator ApplyChillCo(float duration, float speedMultiplier)
    {
        entity.SlowEntity(duration, speedMultiplier);
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
            entityHealth.ReduceHealth(dmgPerTick);
            Instantiate(burnVFX, transform);
            yield return new WaitForSeconds(tickInterval);
        }

        currentStatus = ElementType.None;
    }

    IEnumerator ApplyShockCo(float duration)
    {
        currentStatus = ElementType.Lightning;
        entityFX.PlayStatusVFX(duration, ElementType.Lightning);
        yield return new WaitForSeconds(duration);
        StopShockEffect();
    }

    public bool CanBeApplied(ElementType element)
    {
        if (element == ElementType.Lightning && currentStatus == ElementType.Lightning)
            return true;

        return currentStatus == ElementType.None;
    }
    
}
