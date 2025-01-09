using System;
using UnityEngine;
using Zenject;

public class PlayerAttack : MonoBehaviour
{
    [Inject] private IUserInput _userInput;
    private int _currentAttack = 1;
    private float _timeToAttack;
    private Animator _animator;

    public event Action<int> PlayerAttackButtonDown;

    private void Awake() => _animator = GetComponent<Animator>();

    private void Start()
    {
        _userInput.OnPlayerAttackButtonDown += () =>
        {
            if (_currentAttack == 1)
            {
                PlayerAttackButtonDown?.Invoke(_currentAttack);
                _currentAttack++;
                _timeToAttack = 0;
            }

            else if ((_currentAttack > 1 && _timeToAttack < 1) && !_animator.IsInTransition(0))
            {
                PlayerAttackButtonDown?.Invoke(_currentAttack);

                _currentAttack++;

                if (_currentAttack > 3)
                    _currentAttack = 1;
            }
            if (_timeToAttack > 1)
            {
                PlayerAttackButtonDown?.Invoke(1);
                _timeToAttack = 0;
            }
        };
    }
    private void Update() => _timeToAttack += Time.deltaTime;
}
