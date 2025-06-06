using UnityEngine;
using UnityEngine.UIElements;

public class Player_CounterState : PlayerState
{
    Player_Combat combat;
    bool counteredSomebody;
    public Player_CounterState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = combat.counterLockDuration;
        counteredSomebody = combat.AttemptCounterAttack();
        anim.SetBool("CounterSuccess", counteredSomebody);
        if (counteredSomebody)
            player.sfx.PlayCounter(.5f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (counteredSomebody)
            player.SetVelocity(-player.xDir, rb.linearVelocityY, false);
        else
            player.SetZeroVelocity();

        if (animTriggerCalled)
            stateMachine.ChangeState(player.idleState);

        if (stateTimer < 0 && !counteredSomebody)
            stateMachine.ChangeState(player.idleState);
    }
}
