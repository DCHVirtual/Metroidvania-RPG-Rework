using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_StunnedState : EnemyState
{
    float blinkRate = 0.125f;
    EntityFX fx;
    public Enemy_StunnedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        fx = enemy.GetComponent<EntityFX>();
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 1f; //Stunned Time
        enemy.SetVelocity(-enemy.xDir * enemy.stunForce.x, enemy.stunForce.y, false);
        fx.InvokeRepeating("BlinkRed", 0, blinkRate);
    }

    public override void Exit()
    {
        base.Exit();
        fx.Invoke("CancelBlink",0);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
