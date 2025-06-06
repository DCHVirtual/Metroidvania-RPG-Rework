using Unity.VisualScripting;
using UnityEngine;

public class Player_SwordThrowState : PlayerState
{
    public Player_SwordThrowState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skillManager.swordThrow.EnableDots(true);
    }

    public override void Update()
    {
        base.Update();

        var dirToCursor = DirectionToCursor();

        player.SetZeroVelocity();
        FaceCursor();
        skillManager.swordThrow.PredictTrajectory(dirToCursor);

        if (inputAction.RangeAttack.WasReleasedThisFrame())
        {
            anim.SetBool("ThrowSword", true);
            player.sfx.PlaySwordThrow();
            skillManager.swordThrow.EnableDots(false);
            skillManager.swordThrow.ConfirmTrajectory(dirToCursor);
        }

        if (animTriggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    void FaceCursor()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(player.cursorPos);

        if (Mathf.Sign(cursorPos.x - player.transform.position.x) != player.xDir)
            player.Flip();
    }

    Vector2 DirectionToCursor()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 worldCursorPos = Camera.main.ScreenToWorldPoint(player.cursorPos);

        return (worldCursorPos - playerPos).normalized;
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("ThrowSword", false);
        skillManager.swordThrow.EnableDots(false);
    }
}
