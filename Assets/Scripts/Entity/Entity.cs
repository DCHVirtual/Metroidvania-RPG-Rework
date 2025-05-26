using System;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IAttackerTransform
{
    public event Action OnFlip;
    public int xDir { get; protected set; } = 1;
    public StateMachine stateMachine { get; protected set; }
    public Animator anim { get; protected set; }
    public Rigidbody2D rb { get; protected set; }
    public SpriteRenderer sr { get; protected set; }
    public Entity_StatusHandler statusHandler { get; protected set; }
    public Entity_Stats stats { get; protected set; }

    protected float originalMoveSpeed;
    protected float originalAnimSpeed;
    protected Color originalColor;

    [field: SerializeField] public float moveSpeed { get; protected set; }

    [Header("Collision Detection")]
    [SerializeField] protected float groundCheckDist;
    [SerializeField] protected LayerMask groundMask;
    [SerializeField] protected Transform wallCheck1;
    [SerializeField] protected float wallCheckDist;

    [Header("Knockback")]
    Coroutine knockbackCoroutine;
    bool isStaggered;


    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        statusHandler = GetComponent<Entity_StatusHandler>();
        stats = GetComponent<Entity_Stats>();
        stateMachine = new StateMachine();
    }

    protected virtual void Update()
    {
        stateMachine.UpdateState();
    }

    protected virtual void Start()
    {
        originalMoveSpeed = moveSpeed;
        originalAnimSpeed = anim.speed;
        originalColor = sr.color;
        anim.SetFloat("attackSpeed", 1f);
    }

    public virtual void EntityDeath()
    {

    }

    public virtual void SlowEntity(float duration, float speedMultiplier)
    {
    }

    public void ReceiveKnockback(Vector2 force, float duration)
    {
        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);
        knockbackCoroutine = StartCoroutine(KnockbackCoroutine(force, duration));
    }

    IEnumerator KnockbackCoroutine(Vector2 force, float duration)
    {
        isStaggered = true;
        rb.linearVelocity = force;
        yield return new WaitForSeconds(duration);
        isStaggered = false;
        SetZeroVelocity();
    }

    public void SetVelocity(float x, float y, bool flip = true)
    {
        if (isStaggered) return;

        rb.linearVelocity = new Vector2(x, y);
        if (flip)
            HandleFlip(x);
    }

    public void SetZeroVelocity()
    {
        SetVelocity(0, 0);
    }

    void HandleFlip(float x)
    {
        if (xDir > 0 && x < 0 || xDir < 0 && x > 0)
            Flip();
    }

    public void Flip()
    {
        xDir *= -1;
        transform.localScale = new Vector2(transform.localScale.x * -1, 1);
        OnFlip?.Invoke();
    }

    public virtual bool IsGroundDetected() => Physics2D.Raycast(transform.position, Vector2.down, groundCheckDist, groundMask);
    public virtual bool IsWallDetected()
    {
        return Physics2D.Raycast(wallCheck1.position, xDir * Vector2.right, wallCheckDist, groundMask);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck1.position, wallCheck1.position + new Vector3(xDir * wallCheckDist, 0));
    }

    public Transform GetAttackerTransform()
    {
        return transform;
    }
}
