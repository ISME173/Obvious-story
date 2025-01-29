using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IDestroyable
{
    protected ObjectPool<T> _pool;

    public abstract T Spawn();

    protected abstract void AddListeners(T instance);
    protected abstract void RemoveListeners(IDestroyable destroyable);
}
