using UnityEngine;

public class Enemy_AggroState : EnemyState
{
    protected int moveDir;
    protected float speedMultiplier = 1.5f;

    public Enemy_AggroState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.aggroTime;
        Debug.Log("Entered aggro state!");
    }

    public override void Update()
    {
        base.Update();

        if (enemy.attackTarget == null)
        {
            Debug.Log("No Target!");
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        if (stateTimer < 0 || AggroDistanceExceeded())
            stateMachine.ChangeState(enemy.idleState);

        if (!enemy.WithinAttackRange())
            MoveTowardsPlayer();

        if (enemy.IsPlayerDetected())
            AttemptAttack();
    }

    void MoveTowardsPlayer()
    {
        moveDir = (int)Mathf.Sign(enemy.attackTarget.position.x - enemy.transform.position.x);
        enemy.SetVelocity(moveDir * enemy.moveSpeed * speedMultiplier, rb.linearVelocityY);
    }

    private void AttemptAttack()
    {
        stateTimer = enemy.aggroTime;
        if (enemy.WithinAttackRange() && CanAttack())
            stateMachine.ChangeState(enemy.attackState);
    }

    bool AggroDistanceExceeded() => Vector2.Distance(enemy.transform.position, enemy.attackTarget.position) > enemy.loseAggroDistance;
    bool CanAttack() => Time.time - enemy.lastTimeAttacked >= enemy.attackCooldown;
}
