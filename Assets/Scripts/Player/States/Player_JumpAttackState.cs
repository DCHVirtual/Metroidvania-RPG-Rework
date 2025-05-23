using UnityEngine;

public class Player_JumpAttackState : PlayerState
{
    public Player_JumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.mass = 10000f;
    }

    public override void Exit()
    {
        base.Exit();
        rb.mass = 1f;
    }

    public override void Update()
    {
        base.Update();
        if (player.IsGroundDetected())
        {
            player.anim.SetTrigger("JumpAttackTrigger");
            player.SetVelocity(0, rb.linearVelocityY);
        }

        if (animTriggerCalled)
        {
            player.anim.ResetTrigger("JumpAttackTrigger");
            stateMachine.ChangeState(player.idleState);
        }
    }
}
