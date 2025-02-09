using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class OnDamagableCollideCheck<T> : MonoBehaviour where T : IDamagable
{
    [SerializeField] protected T _ignoreDamagable;
    [SerializeField] protected LayerMask _ignoreLayer;
    [SerializeField] protected bool _isTrigger = true;

    protected Collider2D _collider2D;

    public event Action<IDamagable> OnDamagebleColliderEnter;

    protected virtual void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _collider2D.isTrigger = _isTrigger;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isTrigger == false)
            return;

        if (collision.TryGetComponent(out IDamagable damagable))
        {
            //Debug.Log($"{damagable} collide");
            if (_ignoreDamagable as IDamagable != damagable && collision.gameObject.layer != _ignoreLayer)
            {
                //Debug.Log($"{damagable} is not this object and {damagable} layer is not {_ignoreLayer} layer");
                OnDamagebleColliderEnter?.Invoke(damagable);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isTrigger)
            return;

        if (collision.collider.TryGetComponent(out IDamagable damagable))
        {
            //Debug.Log($"{damagable} collide");
            if (_ignoreDamagable as IDamagable != damagable && collision.gameObject.layer != _ignoreLayer)
            {
                //Debug.Log($"{damagable} is not this object and {damagable} layer is not {_ignoreLayer} layer");
                OnDamagebleColliderEnter?.Invoke(damagable);
            }
        }
    }
}
