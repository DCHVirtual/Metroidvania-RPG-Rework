using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Player : Entity
{
    public static Transform playerTransform;
    #region Declarations
    float originalJumpForce;
    float originalDashSpeed;
    Vector2 originalWallJumpForce;
    Vector2[] originalAttackMovement;

    public float dashDuration { get; private set; } = 0.3f;

    public UI ui { get; private set; }
    

    public PlayerInputSet input { get; private set; }
    public Vector2 moveInput { get; private set; }
    public Vector2 cursorPos { get; private set; }

    public static event Action OnPlayerDeath;
    public Player_SkillManager skillManager { get; private set; }
    public Player_Combat combat { get; private set; }
    public PlayerFX fx { get; private set; }
    public Player_Stats stats { get; protected set; }
    //public Entity_SFX sfx { get; private set; }

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
    public Player_SwordThrowState throwSwordState { get; private set; }
    public Player_DomainState domainState { get; private set; }
    #endregion

    [SerializeField] protected Transform wallCheck2;

    [Header("Ultimate Ability Details")]
    public float riseSpeed = 25;
    public float riseMaxDistance = 3;

    [field: Header("Movement")]
    [field: SerializeField] public float jumpForce { get; private set; }
    [field: SerializeField] public Vector2 wallJumpForce { get; private set; }
    [field: SerializeField] public float airMoveMultiplier { get; private set; }
    [field: SerializeField] public float wallSlideMultiplier { get; private set; }
    [field: SerializeField] public Vector2[] attackMovement {  get; private set; }
    [field: SerializeField] public float dashSpeed {  get; private set; }
    #endregion

    #region Monobehaviors
    protected override void Awake()
    {
        base.Awake();

        ui = FindAnyObjectByType<UI>();
        input = new PlayerInputSet();
        skillManager = GetComponent<Player_SkillManager>();
        fx = GetComponent<PlayerFX>();
        health = GetComponent<Entity_Health>();
        combat = GetComponent<Player_Combat>();
        stats = GetComponent<Player_Stats>();
        //sfx = GetComponent<Entity_SFX>();

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
        throwSwordState = new Player_SwordThrowState(this, stateMachine, "AimSword");
        domainState = new Player_DomainState(this, stateMachine, "Jump");
        playerTransform = transform;
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        //input.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTreeUI();
        
        input.Player.Spell.performed += ctx => skillManager.shard.TryUseSkill();
        input.Player.Spell.performed += ctx => skillManager.timeEcho.TryUseSkill();

        input.Player.Cursor.performed += ctx => cursorPos = ctx.ReadValue<Vector2>();

        input.Player.Interact.performed += ctx => TryInteract();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void TryInteract()
    {
        IInteractable closestInteractable =
            Physics2D.OverlapCircleAll(transform.position, 1.5f)
            .Where(hit => hit.GetComponent<IInteractable>() != null)
            .OrderBy(hit => Vector2.Distance(transform.position, hit.transform.position))
            .Select(hit => hit.GetComponent<IInteractable>())
            .FirstOrDefault();

        if (closestInteractable != null && closestInteractable.CanInteract())
            closestInteractable.Interact();
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
    #endregion

    #region Detections and Gizmos
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
    #endregion

    public void TeleportPlayer(Vector3 pos)
    {
        transform.position = pos;
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deathState);
    }

    /*public override void SlowEntity(float duration, float speedMultiplier)
    {
        StartCoroutine(SlowEntityCo(duration, speedMultiplier));
    }*/

    protected override IEnumerator SlowEntityCo(float duration, float speedMultiplier)
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
