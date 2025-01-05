using System;
using UnityEngine;

public class DesktopInput : MonoBehaviour, IUserInput
{
    [SerializeField] private KeyCode _playerJumpKeyCode;
    [SerializeField] private KeyCode _playerAttackKeyCode;

    public event Action OnPlayerJumpButtonDown;
    public event Action OnPlayerAttackButtonDown;

    private void Update()
    {
        if (Input.GetKeyDown(_playerJumpKeyCode))
            OnPlayerJumpButtonDown?.Invoke();

        if (Input.GetKeyDown(_playerAttackKeyCode))
            OnPlayerAttackButtonDown?.Invoke();
    }

    public float GetPlayerMovingHorizontalInput(float speed)
    {
        return Input.GetAxis("Horizontal") * speed;
    }
    public float GetPlayerMovingVerticalInput(float speed)
    {
        return Input.GetAxis("Vertical") * speed;
    }
}