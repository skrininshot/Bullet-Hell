using System;
using Zenject;

public class StateFactory
{
    public virtual State CreateState(int state)
    {
        throw new NotImplementedException();
    }
}

public abstract class State : IDisposable
{
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void Dispose() { }
}

public abstract class StateMachine : IInitializable, ITickable, IDisposable
{
    public Action<State> OnStateChange;
    public State CurrentState => _currentState;
    protected State _currentState;
    protected StateFactory _factory;

    public virtual void Initialize() { }

    public virtual void Tick() { _currentState?.Update(); }

    public virtual void Dispose() => _currentState?.Dispose();

    public virtual void ChangeState(int state)
    {
        _currentState?.Dispose();
        _currentState = _factory.CreateState(state);
        _currentState.Start();
        OnStateChange?.Invoke(_currentState);
    }
}