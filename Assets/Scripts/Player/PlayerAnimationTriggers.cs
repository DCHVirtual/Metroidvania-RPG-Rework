using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void AnimationTrigger()
    {
        player.stateMachine.currentState.CallAnimationTrigger();
    }

    public void AttackTrigger()
    {

    }
}
