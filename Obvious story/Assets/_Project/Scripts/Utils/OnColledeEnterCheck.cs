using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class OnColledeEnterCheck<T> : MonoBehaviour where T : IDamagable
{
    protected T _ignoreDamagable;

    public event Action<IDamagable> OnCollideEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable damagable))
        {
            if ((IDamagable)_ignoreDamagable != damagable)
                OnCollideEnter?.Invoke(damagable);
        }
    }
}
