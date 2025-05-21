using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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
        
        //If dashed from this state, need to ignore rest of update
        if (stateMachine.currentState == player.dashState) return;

        if (player.moveInput.x != 0 && player.xDir != player.moveInput.x || !player.IsWallDetected())
            stateMachine.ChangeState(player.airState);
        else if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
        else if (rb.linearVelocityY < 0)
        {
            if (player.moveInput.y < 0)
                player.SetVelocity(0, rb.linearVelocityY);
            else
                player.SetVelocity(0, player.wallSlideMultiplier * rb.linearVelocityY);
        }
        if (inputAction.Jump.WasPerformedThisFrame())
            stateMachine.ChangeState(player.wallJumpState);
    }
}