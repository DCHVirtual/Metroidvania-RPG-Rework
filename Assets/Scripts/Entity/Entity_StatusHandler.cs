
using System.Collections;
using UnityEngine;
using Color = UnityEngine.Color;

public class Entity_StatusHandler : MonoBehaviour
{
    [SerializeField] ElementType currentStatus = ElementType.None;
    Entity entity;
    Entity_Stats stats;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        stats = GetComponent<Entity_Stats>();
    }

    public void ApplyChilledStatus(float duration, float speedMultiplier)
    {
        duration *= (1 - stats.GetElementalResistance(ElementType.Ice));
        StartCoroutine(ApplyChillCo(duration, speedMultiplier));
    }

    IEnumerator ApplyChillCo(float duration, float speedMultiplier)
    {
        entity.ChillEntity(duration, speedMultiplier);
        currentStatus = ElementType.Ice;
        yield return new WaitForSeconds(duration);
        currentStatus = ElementType.None;
    }

    public bool CanBeApplied(ElementType element) => currentStatus == ElementType.None;
    
}
