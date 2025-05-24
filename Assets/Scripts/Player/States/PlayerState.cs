using UnityEditor.Tilemaps;
using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet.PlayerActions inputAction;
    protected Player_SkillManager skills;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        this.player = player;
        inputAction = player.input.Player;
        skills = player.GetComponent<Player_SkillManager>();
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
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        if (inputAction.Dash.WasPerformedThisFrame() && CanDash())
        {
            if (player.IsWallDetected())
                player.Flip();
            skills.dash.SetSkillOnCooldown();
            stateMachine.ChangeState(player.dashState);
        }
    }

    bool CanDash()
    {
        if (!skills.dash.CanUseSkill())
            return false;

        if (stateMachine.currentState == player.dashState)
            return false;

        return true;
    }
}
