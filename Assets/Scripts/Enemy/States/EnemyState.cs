using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    protected Entity_Stats stats;
    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy;
        stats = enemy.stats;
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
        enemy.anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocityX));
    }

    public void SyncAttackSpeed()
    {
        anim.SetFloat("attackSpeed", stats.offense.attackSpeed.GetValue());
    }
}
