using UnityEngine;
using UnityEngine.UI;

public class RestartPanel : BasePanel
{
    [Header("UI states")]
    [SerializeField] private Button _restartGame;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _menuButton;

    public Button RestartButton => _restartGame;
    public Button SettingsButton => _settingsButton;
    public Button MenuButton => _menuButton;

    protected override void Init()
    {
        base.Init();
        _restartGame = GetComponent<Button>();
    }
}
