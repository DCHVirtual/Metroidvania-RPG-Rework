using UnityEngine;

public class Enemy_Health : Entity_Health
{
    Enemy enemy;
    Entity_DropManager dropManager;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
        dropManager = GetComponent<Entity_DropManager>();
    }

    protected override void Die()
    {
        base.Die();
        dropManager.DropItems();
        //enemy.EntityDeath();
    }

    public override bool TakeDamage(float dmg, Transform dmgDealer, 
        float elementalDmg = 0f, ElementType elemType = ElementType.None)
    {
        bool damaged = base.TakeDamage(dmg, dmgDealer, elementalDmg, elemType);
        enemy.attackTarget = dmgDealer;
        
        if (enemy.stateMachine.currentState != enemy.aggroState &&
            enemy.stateMachine.currentState != enemy.attackState)
                enemy.stateMachine.ChangeState(enemy.aggroState);
        return damaged;
    }
}
