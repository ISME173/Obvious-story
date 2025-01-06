using System;

public interface IUserInput
{
    public event Action OnPlayerJumpButtonDown;
    public event Action OnPlayerAttackButtonDown;

    public float GetPlayerMovingHorizontalInput(float speed);
}
