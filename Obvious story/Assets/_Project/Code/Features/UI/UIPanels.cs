using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct UIPanels
{
    [SerializeField] private StartPanel _startPanel;
    [SerializeField] private RestartPanel _restartPanel;
    [SerializeField] private LoadingPanel _loadingPanel;
    [SerializeField] private FinishPanel _finishPanel;
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private VictoryPanel _victoryPanel;

    [Header("Any buttons"), Space]
    [SerializeField] private Button _pauseButton;

    public Button PauseButton => _pauseButton;
    public Slider SoundSlider => _settingsPanel.SoundSlider;
    public Slider MusicSlider => _settingsPanel.MusicSlider;

    public StartPanel StartPanel => _startPanel;
    public RestartPanel RestartPanel => _restartPanel;
    public LoadingPanel LoadingPanel => _loadingPanel;
    public FinishPanel FinishPanel => _finishPanel;
    public SettingsPanel SettingsPanel => _settingsPanel;
    public PausePanel PausePanel => _pausePanel;
    public VictoryPanel VictoryPanel => _victoryPanel;

    public void CloseSettings()
    {
        _settingsPanel.Disable(_settingsPanel.DisableAnimTime, SettingsPanelSetActiveFalse);
    }
    private void SettingsPanelSetActiveFalse()
    {
        _settingsPanel.gameObject.SetActive(false);
    }
    public void PauseActivate()
    {
        if (_pausePanel.IsEnable && GameEvents.Instance.IsGameStarting)
            return;

        _pausePanel.Enable();
    }

    public IEnumerator TimerToMethod(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}