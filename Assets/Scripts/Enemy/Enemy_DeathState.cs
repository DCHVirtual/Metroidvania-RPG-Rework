using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy_DeathState : EnemyState
{
    float fadeSpeed = 1f;
    SpriteRenderer sr;
    public Enemy_DeathState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.TurnOffStateMachine();
        enemy.GetComponent<CapsuleCollider2D>().enabled = false;
        sr = enemy.GetComponentInChildren<SpriteRenderer>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (animTriggerCalled)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - fadeSpeed * Time.deltaTime);
            if (sr.color.a <= 0)
                GameObject.Destroy(enemy.gameObject);
        }
    }
}
