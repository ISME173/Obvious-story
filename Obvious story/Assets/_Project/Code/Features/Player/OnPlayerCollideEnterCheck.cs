using System;
using Zenject;

public class OnPlayerCollideEnterCheck : OnColledeEnterCheck<PlayerHealthManager>
{
    [Inject] private PlayerHealthManager _playerHealthManager;

    private void Awake()
    {
        if (_playerHealthManager == null)
            _playerHealthManager = GetComponentInParent<PlayerHealthManager>();

        if (_playerHealthManager == null)
            throw new ArgumentNullException($"{nameof(_playerHealthManager)} is empty");

        _ignoreDamagable = _playerHealthManager;
    }
}
