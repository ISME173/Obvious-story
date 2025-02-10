using System;

public interface IUserInput
{
    public event Action OnPlayerJumpButtonDown, OnPlayerAttackActivate, OnPauseActivate;

    public float GetPlayerMovingHorizontalInput(float speed);
    public DeviceType GetDeviceType();

    public enum DeviceType
    {
        Handler, Desktop
    }
}
