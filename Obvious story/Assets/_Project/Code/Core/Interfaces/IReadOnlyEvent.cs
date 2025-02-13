using UnityEngine.Events;

public interface IReadOnlyEvent
{
    public void AddListener(UnityAction action);
    public void RemoveListener(UnityAction action);
}

public interface IReadOnlyEvent<T>
{
    public void AddListener(UnityAction<T> action);
    public void RemoveListener(UnityAction<T> action);
}