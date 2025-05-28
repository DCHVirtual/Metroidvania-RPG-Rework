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
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        //Input checks
        if (inputAction.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpState);
        else if (inputAction.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.basicAttackState);
        else if (inputAction.CounterAttack.WasPressedThisFrame())
            stateMachine.ChangeState(player.counterState);
        else if (inputAction.RangeAttack.WasPressedThisFrame() && skillManager.swordThrow.CanUseSkill())
            stateMachine.ChangeState(player.throwSwordState);
        else if (inputAction.Ultimate.WasPressedThisFrame())
            CreateDomain();
    }
}
