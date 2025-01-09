using System;
using UnityEngine.UI;

public class MobileInput : IUserInput
{
    private Joystick _joystick;

    public MobileInput(Joystick joystick, Button playerAttackButton)
    {
        _joystick = joystick;
        joystick.PlayerJumpVerticalState += () => 
        {
            OnPlayerJumpButtonDown?.Invoke();
        };

        playerAttackButton.onClick.AddListener((() => 
        {
            OnPlayerAttackButtonDown?.Invoke();
        }));
    }

    public event Action OnPlayerJumpButtonDown;
    public event Action OnPlayerAttackButtonDown;

    public float GetPlayerMovingHorizontalInput(float speed)
    {
        return _joystick.Horizontal * speed;
    }
}
