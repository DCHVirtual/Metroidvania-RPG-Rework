using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (inputAction.Jump.WasPerformedThisFrame())
            stateMachine.ChangeState(player.jumpState);
        else if (inputAction.Attack.WasPerformedThisFrame())
            stateMachine.ChangeState(player.basicAttackState);
    }
}
