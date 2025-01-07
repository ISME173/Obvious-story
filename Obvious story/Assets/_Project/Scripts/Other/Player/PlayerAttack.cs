using System;
using UnityEngine;
using Zenject;

public class PlayerAttack : MonoBehaviour
{
    [Inject] private IUserInput _userInput;

    public event Action PlayerAttackButtonDown;

    private void Start()
    {
        _userInput.OnPlayerAttackButtonDown += () =>
        {
            PlayerAttackButtonDown?.Invoke();
        };
    }
}
