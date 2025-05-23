using UnityEngine;

public class Player_IdleState : Player_GroundedState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
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

        if (player.moveInput.x == player.xDir && player.IsWallDetected())
            return;

        if (player.moveInput.x != 0)
            stateMachine.ChangeState(player.moveState);
        else
            player.SetVelocity(0, rb.linearVelocityY);
    }
}
