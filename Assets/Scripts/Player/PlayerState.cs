using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet.PlayerActions inputAction;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        this.player = player;
        inputAction = player.input.Player;
    }

    public override void Enter()
    {
        base.Enter();
        anim.SetBool(animBoolName, true);
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool(animBoolName, false);
    }

    public override void Update()
    {
        base.Update();
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        if (inputAction.Dash.WasPerformedThisFrame() && CanDashFromState())
            stateMachine.ChangeState(player.dashState);
    }

    bool CanDashFromState()
    {
        return !player.IsWallDetected() && stateMachine.currentState != player.dashState;
    }
}
