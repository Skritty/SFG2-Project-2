using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    State _currentState;
    public State CurrentState => _currentState;

    protected bool InTransition { get; private set; }
    protected State _previousState;

    public virtual void ChangeState<T>() where T : State, new()
    {
        T targetState = new T();
        if (targetState == null) return;
        Initialize(targetState);
        InitiateStateChange(targetState);
    }

    protected virtual void Initialize(State targetState)
    {
        targetState.Initialize();
    }

    public void RevertState()
    {
        if(_previousState != null)
        {
            InitiateStateChange(_previousState);
        }
    }

    private void InitiateStateChange(State targetState)
    {
        if(_currentState != targetState && !InTransition)
        {
            Transition(targetState);
        }
    }

    private void Transition(State newState)
    {
        Debug.Log("State Changed to "+newState.GetType());
        InTransition = true;
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
        InTransition = false;
    }

    private void Update()
    {
        if(_currentState != null)
        {
            _currentState.Tick();
        }
    }
}
