using System.Collections.Generic;

public abstract class States<T>
{
    private Dictionary<T, States<T>> _states = new Dictionary<T, States<T>>();

    public abstract void Awake();

    public abstract void Execute();

    public abstract void Sleep();

    public void AddTransition(T stateKey, States<T> state)
    {
        if (!_states.ContainsKey(stateKey))
            _states.Add(stateKey, state);
    }

    public void RemoveTransition(T stateKey)
    {
        if (_states.ContainsKey(stateKey))
            _states.Remove(stateKey);
    }

    public States<T> GetState(T stateKey)
    {
        if (_states.ContainsKey(stateKey))
            return _states[stateKey];
        return null;
    }
}
