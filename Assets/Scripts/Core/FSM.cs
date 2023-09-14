using System.Threading.Tasks;

public class FSM<T>
{
    public States<T> Current { get => _current; }

    States<T> _current;

    public FSM(States<T> init = null)
    {
        if (init != null) SetInit(init);
    }

    public void SetInit(States<T> init)
    {
        _current = init;
        _current.Awake();
    }

    public void OnUpdate()
    {
        _current.Execute();
    }

    public async void Transition(T input)
    {
        States<T> newState = _current.GetState(input);
        if (newState == null) return;

        _current.Sleep();
        // TODO: Find way to await sleep states
        await Task.Delay(500);
        newState.Awake();
        _current = newState;
    }
}
