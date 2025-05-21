using UnityEngine;

public class Enemy_IdleState : Enemy_GroundedState
{
    protected float idleTimer = 1f;

    public Enemy_IdleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = idleTimer;
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
            if (!enemy.IsGroundDetected() || enemy.IsWallDetected())
                enemy.Flip();
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
