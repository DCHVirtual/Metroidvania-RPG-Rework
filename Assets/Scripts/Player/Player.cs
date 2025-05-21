using System;
using UnityEngine;

public class Player : Entity
{
    public static event Action OnPlayerDeath;
    public PlayerInputSet input { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_AirState airState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeathState deathState { get; private set; }
    public Player_CounterState counterState { get; private set; }

    
    [SerializeField] protected Transform wallCheck2;

    [field: Header("Movement")]
    [field: SerializeField] public float jumpForce { get; private set; }
    [field: SerializeField] public Vector2 wallJumpForce { get; private set; }
    [field: SerializeField] public float airMoveMultiplier { get; private set; }
    [field: SerializeField] public float wallSlideMultiplier { get; private set; }
    [field: SerializeField] public Vector2[] attackMovement {  get; private set; }
    public Vector2 moveInput { get; private set; }



    protected override void Awake()
    {
        base.Awake();
        input = new PlayerInputSet();

        idleState = new Player_IdleState(this, stateMachine, "Idle");
        moveState = new Player_MoveState(this, stateMachine, "Move");
        jumpState = new Player_JumpState(this, stateMachine, "Jump");
        airState = new Player_AirState(this, stateMachine, "Jump");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "Jump");
        dashState = new Player_DashState(this, stateMachine, "Dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "Attack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "JumpAttack");
        deathState = new Player_DeathState(this, stateMachine, "Death");
        counterState = new Player_CounterState(this, stateMachine, "CounterAttempt");
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    protected void Start()
    {
        stateMachine.Initialize(idleState);
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deathState);
    }

    //Either wallcheck must suffice in order to not clip through walls while dashing
    public bool IsWallDetectedDash()
    {
        return Physics2D.Raycast(wallCheck1.position, xDir * Vector2.right, wallCheckDist, groundMask) ||
                 Physics2D.Raycast(wallCheck2.position, xDir * Vector2.right, wallCheckDist, groundMask);
    }

    public override bool IsWallDetected()
    {
        return Physics2D.Raycast(wallCheck1.position, xDir * Vector2.right, wallCheckDist, groundMask) &&
                 Physics2D.Raycast(wallCheck2.position, xDir * Vector2.right, wallCheckDist, groundMask);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDist));
        Gizmos.DrawLine(wallCheck2.position, wallCheck2.position + new Vector3(xDir * wallCheckDist, 0));
    }
}
