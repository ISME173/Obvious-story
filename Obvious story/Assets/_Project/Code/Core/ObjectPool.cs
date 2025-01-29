using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Transform _container;
    private T _prefab;
    private Queue<T> _pool = new();

    public int CountObjectInPool => _pool.Count;

    public ObjectPool(Transform container, T prefab)
    {
        _container = container;
        _prefab = prefab;
    }

    public T Get(out bool isInstantiated)
    {
        T gettingObject = null;

        if (_pool.Count == 0)
        {
            gettingObject = GameObject.Instantiate(_prefab, _container);
            isInstantiated = true;
            return gettingObject;
        }

        gettingObject = _pool.Dequeue();
        gettingObject.gameObject.SetActive(true);
        isInstantiated = false;
        return gettingObject;
    }
    public T Get()
    {
        T gettingObject = null;

        if (_pool.Count == 0)
        {
            gettingObject = GameObject.Instantiate(_prefab, _container);
            return gettingObject;
        }

        gettingObject = _pool.Dequeue();
        gettingObject.gameObject.SetActive(true);
        return gettingObject;
    }

    public void Put(T obj)
    {
        obj.transform.SetParent(_container.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
