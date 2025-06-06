using UnityEngine;
using System.Collections;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : Entity
{
    #region Declarations
    [SerializeField] protected Transform attackCheck;
    [SerializeField] protected float attackCheckDist;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask playerMask;
    [SerializeField] protected LayerMask enemyMask;

    [HideInInspector]public float lastTimeAttacked;

    [field: Header("Attack Details")]
    [field: SerializeField] public float attackCooldown { get; protected set; }
    [field: SerializeField] public float aggroTime { get; protected set; }
    [field: SerializeField] public float loseAggroDistance { get; protected set; }
    [field: SerializeField] public float aggroBehindDist { get; protected set; }

    [field: SerializeField] public Vector2 stunForce { get; protected set; }
    [field: SerializeField] public float stunDuration { get; protected set; }
    public bool counterWindowOpen { get; protected set; }
    [SerializeField] protected GameObject counterImage;

    [HideInInspector] public Transform attackTarget;
    public Entity_Stats stats { get; protected set; }
    #endregion

    #region States
    public Enemy_IdleState idleState { get; protected set; }
    public Enemy_MoveState moveState { get; protected set; }
    public Enemy_AggroState aggroState { get; protected set; }
    public Enemy_AttackState attackState { get; protected set; }
    public Enemy_StunnedState stunnedState { get; protected set; }
    public Enemy_GravitatingState motionlessState { get; protected set; }
    public Enemy_DeathState deathState { get; protected set; }
    #endregion

    #region Monobehavior Functions
    protected override void Awake()
    {
        base.Awake();
        stats = GetComponent<Entity_Stats>();
        idleState = new Enemy_IdleState(this, stateMachine, "Idle");
        moveState = new Enemy_MoveState(this, stateMachine, "Move");
        aggroState = new Enemy_AggroState(this, stateMachine, "Aggro");
        attackState = new Enemy_AttackState(this, stateMachine, "Attack");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "Stunned");
        motionlessState = new Enemy_GravitatingState(this, stateMachine, "Motionless");
        deathState = new Enemy_DeathState(this, stateMachine, "Death");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }
    
    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }
    #endregion

    #region Detection/Collision Functions
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(attackCheck.position, new Vector3(attackCheck.position.x + xDir * attackCheckDist, attackCheck.position.y));
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDist));
    }

    public override bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDist, groundMask);
    public virtual RaycastHit2D IsPlayerDetected() //=> Physics2D.Raycast(wallCheck1.position, Vector2.right * xDir, 50, playerMask);
    {
        RaycastHit2D hit =
         Physics2D.Raycast(wallCheck1.position, Vector2.right * xDir, 15, playerMask | groundMask);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }
    public virtual RaycastHit2D IsPlayerBehind() => Physics2D.Raycast(transform.position, Vector2.left * xDir, aggroBehindDist, playerMask);
    public virtual bool WithinAttackRange() => Physics2D.Raycast(attackCheck.position, Vector2.right * xDir, attackCheckDist, playerMask);
    public virtual bool IsEnemyDetected() => Physics2D.Raycast(wallCheck1.position, xDir * Vector2.right, wallCheckDist, enemyMask);

    #endregion

    #region Counter/Stun Window Functions
    public bool DetectCounter()
    {
        if (counterWindowOpen)
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public void OpenCounterWindow()
    {
        counterWindowOpen = true;
        counterImage.SetActive(true);
    }
    public void CloseCounterWindow()
    {
        counterWindowOpen = false;
        counterImage.SetActive(false);
    }
    #endregion

    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(deathState);
    }

    void HandlePlayerDeath()
    {
        if (attackTarget != null)
            StartCoroutine(EnterIdleAfterTime(.5f));
    }

    IEnumerator EnterIdleAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        stateMachine.ChangeState(idleState);
    }

    IEnumerator FreezeTime(float _seconds)
    {
        stateMachine.ChangeState(motionlessState);
        yield return new WaitForSeconds(_seconds);
        stateMachine.ChangeState(idleState);
    }

    /*public override void SlowEntity(float duration, float speedMultiplier, bool canOverride = false)
    {
        base.SlowEntity(duration, speedMultiplier, canOverride);
    }*/

    protected override IEnumerator SlowEntityCo(float duration, float speedMultiplier)
    {
        moveSpeed = originalMoveSpeed * speedMultiplier;
        anim.speed = originalAnimSpeed * speedMultiplier;

        yield return new WaitForSeconds(duration);

        slowDownCo = null;

        moveSpeed = originalMoveSpeed;
        anim.speed = originalAnimSpeed;
    }
}
