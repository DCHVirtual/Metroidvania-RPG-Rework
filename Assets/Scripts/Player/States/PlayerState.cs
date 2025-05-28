using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet.PlayerActions inputAction;
    protected Player_SkillManager skillManager;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        this.player = player;
        inputAction = player.input.Player;
        skillManager = player.GetComponent<Player_SkillManager>();
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
            skillManager.dash.SetSkillOnCooldown();
            stateMachine.ChangeState(player.dashState);
        }
        
    }

    protected void CreateDomain()
    {
        if (CanCreateDomain())
        {
            if (skillManager.domain.InstantDomain())
                skillManager.domain.CreateDomain();
            else
                stateMachine.ChangeState(player.domainState);

            skillManager.domain.SetSkillOnCooldown();
        }
    }

    bool CanCreateDomain()
    {
        if (!skillManager.domain.CanUseSkill())
            return false;

        if (stateMachine.currentState == player.dashState ||
            stateMachine.currentState == player.basicAttackState)
            return false;

        return true;
    }

    bool CanDash()
    {
        if (!skillManager.dash.CanUseSkill())
            return false;

        if (stateMachine.currentState == player.dashState ||
            stateMachine.currentState == player.domainState)
            return false;

        return true;
    }
}
