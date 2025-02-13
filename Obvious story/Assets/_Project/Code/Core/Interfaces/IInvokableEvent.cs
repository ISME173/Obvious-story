public interface IInvokableEvent : IReadOnlyEvent
{
    void Invoke();
}

public interface IInvokableEvent<T> : IReadOnlyEvent<T>
{
    void Invoke(T argument);
}