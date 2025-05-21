using UnityEngine;

public class StateMachine
{
    public EntityState currentState { get; private set; }
    bool canChangeState;

    public void Initialize(EntityState _startState)
    {
        canChangeState = true;
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(EntityState _newState)
    {
        if (canChangeState)
        {
            currentState.Exit();
            currentState = _newState;
            currentState.Enter();
        }
    }

    public void UpdateState()
    {
        currentState.Update();
    }

    public void TurnOffStateMachine() => canChangeState = false;
    public void TurnOnStateMachine() => canChangeState = true;
}
