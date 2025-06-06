using UnityEngine;

public class Player_AirState : PlayerState
{
    public Player_AirState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //Ground Check
        if (player.IsGroundDetected()) 
        {
            player.sfx.PlayJumpLand(.4f);
            player.SetZeroVelocity();
            stateMachine.ChangeState(player.idleState);
            return;
        }

        //Air Movement
        if (player.moveInput.x != 0)
            player.SetVelocity(player.moveSpeed * player.airMoveMultiplier * player.moveInput.x, rb.linearVelocityY);

        //Wall Slide Check
        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }

        //Input checks
        if (inputAction.Attack.WasPerformedThisFrame())
        {
            player.sfx.PlayAttackSwing(.5f);
            stateMachine.ChangeState(player.jumpAttackState);
        }
        else if (inputAction.Ultimate.WasPressedThisFrame())
            CreateDomain();
    }
}
