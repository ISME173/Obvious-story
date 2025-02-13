using UnityEngine.Events;

public class SecureUnityEvent : IInvokableEvent
{
    private readonly UnityEvent _event = new UnityEvent();

    public void AddListener(UnityAction action) => _event.AddListener(action);
    public void RemoveListener(UnityAction action) => _event.RemoveListener(action);
    public void Invoke() => _event.Invoke();
}

public class SecureUnityEvent<T> : IInvokableEvent<T>
{
    private readonly UnityEvent<T> _event = new UnityEvent<T>();

    public void AddListener(UnityAction<T> action) => _event.AddListener(action);
    public void RemoveListener(UnityAction<T> action) => _event.RemoveListener(action);
    public void Invoke(T arg) => _event.Invoke(arg);
}
