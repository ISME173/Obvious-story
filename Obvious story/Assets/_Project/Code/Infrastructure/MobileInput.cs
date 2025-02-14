using System;
using UnityEngine.UI;

public class MobileInput : IUserInput
{
    private Joystick _joystick;
    private Button _playerAttackButton, _pauseButton;

    public MobileInput(Joystick joystick, Button playerAttackButton, Button pauseButton)
    {
        _joystick = joystick;
        _playerAttackButton = playerAttackButton;
        _pauseButton = pauseButton;

        _joystick.PlayerJumpVerticalState += () => 
        {
            OnPlayerJumpButtonDown?.Invoke();
        };

        _playerAttackButton.onClick.AddListener(() => 
        {
            OnPlayerAttackActivate?.Invoke();
        });

        _pauseButton.onClick.AddListener(() =>
        {
            OnPauseActivate?.Invoke();
        });
    }

    public event Action OnPlayerJumpButtonDown, OnPlayerAttackActivate, OnPauseActivate;

    public float GetPlayerMovingHorizontalInput(float speed) => _joystick.Horizontal * speed;
    public IUserInput.DeviceType GetDeviceType() => IUserInput.DeviceType.Handler;

    public void UISetActiveFalse()
    {
        _joystick.gameObject.SetActive(false);
        _pauseButton.gameObject.SetActive(false);
        _playerAttackButton?.gameObject.SetActive(false);
    }
    public void UISetActiveTrue()
    {
        _joystick.gameObject.SetActive(true);
        _pauseButton.gameObject.SetActive(true);
        _playerAttackButton.gameObject.SetActive(true);
    }
}
