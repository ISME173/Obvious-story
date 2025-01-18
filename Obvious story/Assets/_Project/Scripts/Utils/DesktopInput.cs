using System;
using UnityEngine;

public class DesktopInput : MonoBehaviour, IUserInput
{
    [SerializeField] private KeyCode _playerJumpKeyCode;
    [SerializeField] private KeyCode _playerAttackKeyCode;

    public event Action OnPlayerJumpButtonDown;
    public event Action OnPlayerAttackActivate;

    private void Update()
    {
        if (Input.GetKeyDown(_playerJumpKeyCode))
            OnPlayerJumpButtonDown?.Invoke();

        if (Input.GetKeyDown(_playerAttackKeyCode))
            OnPlayerAttackActivate?.Invoke();
    }

    public float GetPlayerMovingHorizontalInput(float speed)
    {
        return Input.GetAxis("Horizontal") * speed;
    }
}