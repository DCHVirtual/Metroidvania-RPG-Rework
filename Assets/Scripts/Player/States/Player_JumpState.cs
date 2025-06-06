using UnityEngine;

public class Player_JumpState : PlayerState
{

    float forcedJumpTime = 0.1f;
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.sfx.PlayJump(.5f);
        stateTimer = forcedJumpTime;
        player.SetVelocity(player.moveInput.x * player.moveSpeed, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(player.airState);
    }
}
