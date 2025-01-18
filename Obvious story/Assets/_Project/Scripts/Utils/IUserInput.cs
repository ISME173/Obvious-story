using System;

public interface IUserInput
{
    public event Action OnPlayerJumpButtonDown;
    public event Action OnPlayerAttackActivate;

    public float GetPlayerMovingHorizontalInput(float speed);
}
