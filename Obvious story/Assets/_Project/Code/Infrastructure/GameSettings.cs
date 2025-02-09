using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObject/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private string[] _levelNames;

    [Header("Desktop input settings"), Space]
    [SerializeField] private KeyCode _playerAttackKeyCode;
    [SerializeField] private KeyCode _playerJumpKeyCode;
    [SerializeField] private KeyCode _pauseKeyCode;

    public string[] LevelNames => _levelNames;
    public KeyCode PlayerAttackKeyCode => _playerAttackKeyCode;
    public KeyCode PlayerJumpKeyCode => _playerJumpKeyCode;
    public KeyCode PauseKeyCode => _pauseKeyCode;
}
