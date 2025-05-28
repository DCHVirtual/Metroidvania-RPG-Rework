using UnityEngine;

public class Enemy_MoveState : Enemy_GroundedState
{
    public Enemy_MoveState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
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

        enemy.SetVelocity(enemy.moveSpeed * enemy.xDir, enemy.rb.linearVelocityY);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected() || enemy.IsEnemyDetected())
        {
            enemy.SetZeroVelocity();
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
