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
            throw new ArgumentNullException("_ignoreEnemyDamagable is empty");

        _ignoreDamagable = _ignoreEnemyDamagable;
    }
}
