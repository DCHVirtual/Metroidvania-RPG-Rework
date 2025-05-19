using System;
using UnityEngine;

public class Player : Entity
{
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


    [field: Header("Movement")]
    [field: SerializeField] public float moveSpeed { get; private set; }
    [field: SerializeField] public float jumpForce { get; private set; }
    [field: SerializeField] public Vector2 wallJumpForce { get; private set; }
    [field: SerializeField] public float airMoveMultiplier { get; private set; }
    [field: SerializeField] public float wallSlideMultiplier { get; private set; }
    [field: SerializeField] public Vector2[] attackMovement {  get; private set; }
    public Vector2 moveInput { get; private set; }
    

    void Awake()
    {
        input = new PlayerInputSet();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
        idleState = new Player_IdleState(this, stateMachine, "Idle");
        moveState = new Player_MoveState(this, stateMachine, "Move");
        jumpState = new Player_JumpState(this, stateMachine, "Jump");
        airState = new Player_AirState(this, stateMachine, "Jump");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "Jump");
        dashState = new Player_DashState(this, stateMachine, "Dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "Attack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "JumpAttack");
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

    protected override void Start()
    {
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
