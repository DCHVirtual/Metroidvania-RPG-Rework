using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player_BasicAttackState : PlayerState
{
    int comboCounter;
    int numComboAttacks = 3;
    bool attackedBeforeEnd;
    float attackVelocityTime = 0.15f;

    public Player_BasicAttackState(Player _player, StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, rb.linearVelocityY);
        stateTimer = attackVelocityTime;
        if (comboCounter == numComboAttacks || !attackedBeforeEnd)
            comboCounter = 0;
        player.anim.SetInteger("comboCounter", comboCounter);
        float attackDir = player.moveInput.x == 0 ? player.xDir : player.moveInput.x;
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
        attackedBeforeEnd = false;
    }

    public override void Exit()
    {
        base.Exit();
        
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            player.SetZeroVelocity();
        CheckForAttack();
        if (animTriggerCalled)
        {
            if (attackedBeforeEnd && comboCounter != numComboAttacks)
                stateMachine.ChangeState(player.basicAttackState);
            else
                stateMachine.ChangeState(player.idleState);
        }
    }

    void CheckForAttack()
    {
        if (!attackedBeforeEnd && inputAction.Attack.WasPerformedThisFrame())
        {
            attackedBeforeEnd = true;
            comboCounter++;
        }
    }
}
