using UnityEngine;

public class Player_DomainState : PlayerState
{
    Vector2 originalPosition;
    float originalGravity;
    float finalRiseDistance;
    bool isLevitating, createdDomain;

    public Player_DomainState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        originalPosition = player.transform.position;
        originalGravity = rb.gravityScale;
        finalRiseDistance = GetAvailableRiseDistance();

        player.SetVelocity(0, player.riseSpeed);
    }

    float GetAvailableRiseDistance()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, 
            Vector2.up, player.riseMaxDistance, player.groundMask);

        return hit.collider != null ? hit.distance - 1 : player.riseMaxDistance;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = originalGravity;
        isLevitating = false;
        createdDomain = false;
        player.GetComponent<CapsuleCollider2D>().enabled = true;
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(
            originalPosition, player.transform.position) > finalRiseDistance &&
            isLevitating == false)
        {
            Levitate();
        }

        if (isLevitating)
        {
            skillManager.domain.CastSpells();

            if (stateTimer < 0)
                stateMachine.ChangeState(player.idleState);
        }
    }

    void Levitate()
    {
        player.GetComponent<CapsuleCollider2D>().enabled = false;
        isLevitating = true;
        player.SetZeroVelocity();
        rb.gravityScale = 0;

        stateTimer = skillManager.domain.GetDuration();

        if (!createdDomain)
        {
            createdDomain = true;
            skillManager.domain.CreateDomain();
        }
    }
}
