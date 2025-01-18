using System;
using UnityEngine;

public class OnPlayerCollideEnterCheck : OnColledeEnterCheck<PlayerHealthManager>
{
    [SerializeField] private PlayerHealthManager _playerHealthManager;

    private void Awake()
    {
        if (_playerHealthManager == null)
            _playerHealthManager = GetComponentInParent<PlayerHealthManager>();

        if (_playerHealthManager == null)
            throw new ArgumentNullException("_playerHealthManager is empty");

        _ignoreDamagable = _playerHealthManager;
    }
}
