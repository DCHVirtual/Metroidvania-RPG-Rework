using UnityEngine;

public class EnemySkeleton : Enemy, ICounterable
{
    public bool WasCountered()
    {
        if (counterWindowOpen)
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
