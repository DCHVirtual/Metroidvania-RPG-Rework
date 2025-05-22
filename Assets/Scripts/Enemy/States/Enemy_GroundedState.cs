using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_GroundedState : EnemyState
{
    Transform player;

    public Enemy_GroundedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        RaycastHit2D hit;
        if ((hit = enemy.IsPlayerDetected()) || (hit = enemy.IsPlayerBehind()))
        {
            enemy.attackTarget = hit.transform;
            stateMachine.ChangeState(enemy.aggroState);
        }
    }
}
