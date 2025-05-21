using UnityEngine;

public class Enemy_Health : Entity_Health
{
    Enemy enemy;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
    }

    public override void Die()
    {
        base.Die();
        enemy.EntityDeath();
    }

    public override void TakeDamage(float dmg, Transform dmgDealer)
    {
        base.TakeDamage(dmg, dmgDealer);
        enemy.attackTarget = dmgDealer;
        if (enemy.stateMachine.currentState != enemy.aggroState &&
            enemy.stateMachine.currentState != enemy.attackState)
                enemy.stateMachine.ChangeState(enemy.aggroState);
    }
}
