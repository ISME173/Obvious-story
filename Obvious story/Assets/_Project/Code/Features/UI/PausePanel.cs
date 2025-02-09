using UnityEngine;
using UnityEngine.UI;

public class PausePanel : BasePanel
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _menuButton;

    public Button ContinueButton => _continueButton;
    public Button SettingsButton => _settingsButton;
    public Button MenuButton => _menuButton;
}
