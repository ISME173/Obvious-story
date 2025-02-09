using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DesktopInput : MonoBehaviour, IUserInput
{
    [Inject] private GameSettings _gameSettings;
    private Dictionary<KeyCode, Action> _keyCodeToEvent = new Dictionary<KeyCode, Action>();

    public event Action OnPlayerAttackActivate, OnPauseActivate, OnPlayerJumpButtonDown;

    private void Start()
    {
        _keyCodeToEvent.Add(_gameSettings.PauseKeyCode, OnPauseActivate);
        _keyCodeToEvent.Add(_gameSettings.PlayerJumpKeyCode, OnPlayerJumpButtonDown);
        _keyCodeToEvent.Add(_gameSettings.PlayerAttackKeyCode, OnPlayerAttackActivate);
    }

    private void Update()
    {
        foreach (var keyAction in _keyCodeToEvent)
        {
            if (Input.GetKeyDown(keyAction.Key))
                keyAction.Value?.Invoke();
        }
    }

    public float GetPlayerMovingHorizontalInput(float speed)
    {
        return Input.GetAxis("Horizontal") * speed;
    }
}