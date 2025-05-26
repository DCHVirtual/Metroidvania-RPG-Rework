using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    Player player;
    Entity_Combat entityCombat => GetComponentInParent<Entity_Combat>();

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void AnimationTrigger()
    {
        player.stateMachine.currentState.CallAnimationTrigger();
    }

    public void ThrowSword()
    {
        player.skillManager.swordThrow.ThrowSword();
    }

    public void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }
}
