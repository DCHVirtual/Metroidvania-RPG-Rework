using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyAnimationTriggers : MonoBehaviour
{
    Enemy enemy => GetComponentInParent<Enemy>();
    Entity_Combat entityCombat => GetComponentInParent<Entity_Combat>();

    public void AnimationTrigger()
    {
        enemy.stateMachine.currentState.CallAnimationTrigger();
    }
    public void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }

    void OpenCounterWindowTrigger() => enemy.OpenCounterWindow();
    void CloseCounterWindowTrigger() => enemy.CloseCounterWindow();
}
