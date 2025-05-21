using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
        enemy.CloseCounterWindow();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0, rb.linearVelocityY);
        if (animTriggerCalled)
            stateMachine.ChangeState(enemy.aggroState);
    }
}
