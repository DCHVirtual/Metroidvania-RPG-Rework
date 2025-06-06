using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player_DashState : PlayerState
{
    float originalGravityScale;

    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.sfx.PlayDash(.5f);

        skillManager.dash.OnStartEffect();
        player.fx.AfterImageEffect(player.dashDuration);

        player.GetComponent<CapsuleCollider2D>().enabled = false;
        stateTimer = player.dashDuration;//player.skill.dash.duration;

        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        skillManager.dash.OnEndEffect();

        player.GetComponent<CapsuleCollider2D>().enabled = true;
        player.SetVelocity(0f, rb.linearVelocityY);

        rb.gravityScale = originalGravityScale;
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(/*SkillManager.instance.dash.speed*/ player.dashSpeed * player.xDir, 0);
        if (stateTimer < 0)
        {
            if (player.IsGroundDetected())
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.airState);
        }

        if (player.IsWallDetectedDash())
        {
            if (player.IsGroundDetected())
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
