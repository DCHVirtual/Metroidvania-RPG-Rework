using UnityEngine;

public class PlayerAnimationTriggers : Entity_AnimationTriggers
{
    Player player;
    Entity_Combat entityCombat;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        entityCombat = GetComponentInParent<Entity_Combat>();
    }

    public void AnimationTrigger()
    {
        player.stateMachine.currentState.CallAnimationTrigger();
    }

    public void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }

    public void ThrowSword()
    {
        player.skillManager.swordThrow.ThrowSword();
    }

}
