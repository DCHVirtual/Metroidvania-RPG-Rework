using UnityEngine;

public class Enemy_GravitatingState : EnemyState
{
    float defaultGravityScale;
    public Enemy_GravitatingState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravityScale = rb.gravityScale;
        //rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravityScale;
        //rb.constraints = RigidbodyConstraints2D.None;
        //rb.freezeRotation = true;
    }

    public override void Update()
    {
        base.Update();
    }
}
