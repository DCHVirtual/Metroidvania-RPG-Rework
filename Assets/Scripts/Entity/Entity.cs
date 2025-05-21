using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int xDir { get; protected set; } = 1;
    public StateMachine stateMachine { get; protected set; }
    public Animator anim { get; protected set; }
    public Rigidbody2D rb { get; protected set; }

    [field: SerializeField] public float moveSpeed { get; private set; }

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
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
    }

    protected virtual void Update()
    {
        stateMachine.UpdateState();
    }

    public virtual void EntityDeath()
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
}
