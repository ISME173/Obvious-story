using System;
using UnityEngine.UI;

public class MobileInput : IUserInput
{
    private Joystick _joystick;

    public MobileInput(Joystick joystick, Button playerAttackButton, Button pauseButton)
    {
        _joystick = joystick;
        joystick.PlayerJumpVerticalState += () => 
        {
            OnPlayerJumpButtonDown?.Invoke();
        };

        playerAttackButton.onClick.AddListener(() => 
        {
            OnPlayerAttackActivate?.Invoke();
        });

        pauseButton.onClick.AddListener(() =>
        {
            OnPauseActivate?.Invoke();
        });
    }

    public event Action OnPlayerJumpButtonDown, OnPlayerAttackActivate, OnPauseActivate;

    public float GetPlayerMovingHorizontalInput(float speed)
    {
        return _joystick.Horizontal * speed;
    }
}
