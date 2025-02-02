using System;
using UnityEngine;

public class OnEnemyCollideEnterCheck : OnColledeEnterCheck<Enemy>
{
    [SerializeField] private Enemy _ignoreEnemyDamagable;

    private void Awake()
    {
        if (_ignoreEnemyDamagable == null)
            _ignoreEnemyDamagable = GetComponentInParent<Enemy>();

        if (_ignoreEnemyDamagable == null)
            throw new ArgumentNullException($"{nameof(_ignoreEnemyDamagable)} is empty");

        _ignoreDamagable = _ignoreEnemyDamagable;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (_ignoreDamagable.CanTakeDamageAnyEnemy == false)
        {
            if (collision.TryGetComponent(out IDamagable damagable) && damagable as Enemy == false)
                base.OnTriggerEnter2D(collision);
        }
        else
        {
            base.OnTriggerEnter2D(collision);
        }
    }
}
