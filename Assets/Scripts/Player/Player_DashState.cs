using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player_DashState : PlayerState
{
    float dashDuration = 0.3f;
    float dashSpeed = 20f;

    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.GetComponent<CapsuleCollider2D>().enabled = false;
        stateTimer = dashDuration;//player.skill.dash.duration;
    }

    public override void Exit()
    {
        base.Exit();
        player.GetComponent<CapsuleCollider2D>().enabled = true;
        player.SetVelocity(0f, rb.linearVelocityY);
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(/*SkillManager.instance.dash.speed*/ dashSpeed * player.xDir, 0);
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
