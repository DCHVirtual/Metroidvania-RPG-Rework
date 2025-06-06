using UnityEngine;
using UnityEngine.InputSystem;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName;
    protected Animator anim;
    protected Rigidbody2D rb;

    protected float stateTimer;
    protected bool animTriggerCalled;

    public EntityState(Entity entity, StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        anim = entity.anim;
        rb = entity.rb;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        animTriggerCalled = false;
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public void CallAnimationTrigger()
    {
        animTriggerCalled = true;
    }

    public override string ToString()
    {
        return animBoolName;
    }
}
