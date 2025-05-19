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
        if (/*rb.linearVelocityY <= 0.01 && */player.IsGroundDetected()) 
        {
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

        if (inputAction.Attack.WasPerformedThisFrame())
            stateMachine.ChangeState(player.jumpAttackState);
        /*
        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            stateMachine.ChangeState(player.aimSwordState);
        else if (Input.GetKeyDown(KeyCode.Q) && player.skill.blackhole.CheckUseSkill())
            stateMachine.ChangeState(player.blackholeState);

        DashCheck();*/
    }
}
