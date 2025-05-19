using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int xDir { get; protected set; } = 1;
    public StateMachine stateMachine { get; protected set; }
    public Animator anim { get; protected set; }
    public Rigidbody2D rb { get; protected set; }

    [Header("Collision Detection")]
    [SerializeField] protected float groundCheckDist;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Transform wallCheck1;
    [SerializeField] protected Transform wallCheck2;
    [SerializeField] protected float wallCheckDist;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        stateMachine.UpdateState();
    }

    public void SetVelocity(float x, float y, bool flip = true)
    {
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

    public bool IsGroundDetected() => Physics2D.Raycast(transform.position, Vector2.down, groundCheckDist, groundLayer);
    public bool IsWallDetected()
    {
        return Physics2D.Raycast(wallCheck1.position, xDir * Vector2.right, wallCheckDist, groundLayer) &&
                 Physics2D.Raycast(wallCheck2.position, xDir * Vector2.right, wallCheckDist, groundLayer);
    }
    protected void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDist));
        Gizmos.DrawLine(wallCheck1.position, wallCheck1.position + new Vector3(xDir * wallCheckDist, 0));
        Gizmos.DrawLine(wallCheck2.position, wallCheck2.position + new Vector3(xDir * wallCheckDist, 0));
    }
}
