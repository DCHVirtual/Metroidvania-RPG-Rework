using UnityEngine;

public class Player_DeathState : PlayerState
{
    public Player_DeathState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.simulated = false;
        stateMachine.TurnOffStateMachine();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
