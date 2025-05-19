using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player_WallJumpState : PlayerState
{
    float forcedJumpTime = 0.2f;
    public Player_WallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(player.wallJumpForce.x * -player.xDir, player.wallJumpForce.y);
        stateTimer = forcedJumpTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
