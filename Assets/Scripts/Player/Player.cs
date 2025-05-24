using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{

    #region Declarations
    float originalJumpForce;
    float originalDashSpeed;
    Vector2 originalWallJumpForce;
    Vector2[] originalAttackMovement;

    UI ui;

    public static event Action OnPlayerDeath;
    public PlayerInputSet input { get; private set; }
    public Player_SkillManager skillManager { get; private set; }

    #region States
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
    #endregion

    [SerializeField] protected Transform wallCheck2;

    [field: Header("Movement")]
    [field: SerializeField] public float jumpForce { get; private set; }
    [field: SerializeField] public Vector2 wallJumpForce { get; private set; }
    [field: SerializeField] public float airMoveMultiplier { get; private set; }
    [field: SerializeField] public float wallSlideMultiplier { get; private set; }
    [field: SerializeField] public Vector2[] attackMovement {  get; private set; }
    [field: SerializeField] public float dashSpeed {  get; private set; }
    public Vector2 moveInput { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        ui = FindAnyObjectByType<UI>();
        input = new PlayerInputSet();
        skillManager = GetComponent<Player_SkillManager>();

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

        input.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTreeUI();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        originalJumpForce = jumpForce;
        originalWallJumpForce = wallJumpForce;
        originalAttackMovement = attackMovement;
        originalDashSpeed = dashSpeed;
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

    public override void SlowEntity(float duration, float speedMultiplier)
    {
        StartCoroutine(SlowEntityCo(duration, speedMultiplier));
    }

    IEnumerator SlowEntityCo(float duration, float speedMultiplier)
    {
        moveSpeed *= speedMultiplier;
        jumpForce *= speedMultiplier;
        anim.speed *= speedMultiplier;
        wallJumpForce *= speedMultiplier;
        dashSpeed *= speedMultiplier;

        for (int i = 0; i < attackMovement.Length; i++)
            attackMovement[i] *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        anim.speed = originalAnimSpeed;
        wallJumpForce = originalWallJumpForce;
        dashSpeed = originalDashSpeed;

        for (int i = 0; i < attackMovement.Length; i++)
            attackMovement[i] = originalAttackMovement[i];
    }
}
