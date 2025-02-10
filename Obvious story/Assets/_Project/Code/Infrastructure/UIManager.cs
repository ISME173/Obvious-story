using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class UIManager : MonoBehaviour
{
    [Header("Panels"), Space]
    [SerializeField] private StartPanel _startPanel;
    [SerializeField] private RestartPanel _restartPanel;
    [SerializeField] private LoadingPanel _loadingPanel;
    [SerializeField] private FinishPanel _finishPanel;
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private VictoryPanel _victoryPanel;

    [Header("States for panels"), Space]
    [SerializeField, Min(0)] private float _timeToActivateRestartPanelBeforePlayerDied = 1.5f;

    [Header("Any buttons"), Space]
    [SerializeField] private Button _pauseButton;

    [Inject] private IUserInput _userInput;
    private MobileInput _mobileInput;

    private static UIManager _instance;

    [HideInInspector]
    public UnityEvent ButtonPlayClick, ButtonRestartClick, ButtonSettingsClick, ButtonNextLevelClick, ButtonContinueClick,
        MenuButtonClick, ButtonPauseClick;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<UIManager>();

                if (_instance == null)
                {
                    GameObject uIManager = new GameObject();
                    _instance = uIManager.AddComponent<UIManager>();
                    _instance.name = typeof(UIManager).ToString();
                    return _instance;
                }
            }
            return _instance;
        }
    }

    public Slider SoundSlider => _settingsPanel.SoundSlider;
    public Slider MusicSlider => _settingsPanel.MusicSlider;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;

        if (_userInput.GetDeviceType() == IUserInput.DeviceType.Handler)
            _mobileInput = _userInput as MobileInput;

        AddAllListeners();
    }
    private void OpenSettings()
    {
        SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick);

        if (_userInput.GetDeviceType() == IUserInput.DeviceType.Handler)
            _mobileInput.UISetActiveFalse();

        _settingsPanel.Enable();
    }
    private void CloseSettings()
    {
        SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick);
        _settingsPanel.Disable();
        StartCoroutine(TimerToMethod(_settingsPanel.DisableAnimTime, () => { _settingsPanel.gameObject.SetActive(false); }));
    }
    private void RestartButtonClick()
    {
        SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick);
        ButtonRestartClick?.Invoke();
        _restartPanel.Disable();
    }
    private void OpenMenu()
    {
        SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick);

        if (_userInput.GetDeviceType() == IUserInput.DeviceType.Handler)
            _mobileInput.UISetActiveFalse();

        MenuButtonClick?.Invoke();
    }
    private void PauseActivate()
    {
        if (_pausePanel.IsEnable && GameManager.Instance.IsGameStarting)
            return;

        SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick);

        if (_userInput.GetDeviceType() == IUserInput.DeviceType.Handler)
            _mobileInput.UISetActiveFalse();

        ButtonPauseClick?.Invoke();
        _pausePanel.Enable();
    }
    private void FinishPanelDisable()
    {
        SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick);
        _finishPanel.Disable();
    }
    private void ExitGame()
    {
        Application.Quit();
    }
    private void AddAllListeners()
    {
        _userInput.OnPauseActivate += PauseActivate;

        _startPanel.StartGame.onClick.AddListener(() =>
        {
            SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick);
            ButtonPlayClick?.Invoke();
            _startPanel.Disable();
        });
        _startPanel.Settings.onClick.AddListener(OpenSettings);
        _startPanel.ExitGame.onClick.AddListener(ExitGame);

        _restartPanel.RestartButton.onClick.AddListener(RestartButtonClick);
        _restartPanel.SettingsButton.onClick.AddListener(OpenSettings);
        _restartPanel.MenuButton.onClick.AddListener(OpenMenu);

        _settingsPanel.ExitSettings.onClick.AddListener(CloseSettings);

        _victoryPanel.MenuButton.onClick.AddListener(OpenMenu);

        _pausePanel.MenuButton.onClick.AddListener(OpenMenu);
        _pauseButton.onClick.AddListener(PauseActivate);
        _pausePanel.SettingsButton.onClick.AddListener(OpenSettings);
        _pausePanel.ContinueButton.onClick.AddListener(() =>
        {
            if (_pausePanel.IsEnable == false)
                return;

            SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick);

            if (_userInput.GetDeviceType() == IUserInput.DeviceType.Handler)
                _mobileInput.UISetActiveTrue();

            ButtonContinueClick?.Invoke();
            _pausePanel.Disable();
        });

        _finishPanel.NextLevelButton.onClick.AddListener(() =>
        {
            SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick);
            ButtonNextLevelClick?.Invoke();
        });
        _finishPanel.ReturnButton.onClick.AddListener(FinishPanelDisable);

        GameManager.Instance.OnSceneLoaoded.AddListener((scene) =>
        {
            if (GameManager.Instance.IsFirstGameOpen)
            {
                _startPanel.Enable();

                if (_userInput.GetDeviceType() == IUserInput.DeviceType.Handler)
                    _mobileInput.UISetActiveFalse();
            }
            else
            {
                if (_userInput.GetDeviceType() == IUserInput.DeviceType.Handler)
                    _mobileInput.UISetActiveTrue();
            }
        });

        GameManager.Instance.OnGameOpen.AddListener(() =>
        {
            if (_userInput.GetDeviceType() == IUserInput.DeviceType.Handler)
                _mobileInput.UISetActiveFalse();

            if (GameManager.Instance.IsFirstGameOpen)
                _startPanel.Enable();
        });

        GameManager.Instance.OnPlay.AddListener(() =>
        {
            if (_userInput.GetDeviceType() == IUserInput.DeviceType.Handler)
                _mobileInput.UISetActiveTrue();
        });

        GameManager.Instance.OnPlayerFinishExit.AddListener(() =>
        {
            if (_finishPanel.IsEnable == true)
                _finishPanel.Disable();
        });

        GameManager.Instance.OnGameOver.AddListener(() => { StartCoroutine(TimerToMethod(_timeToActivateRestartPanelBeforePlayerDied, _restartPanel.Enable)); });

        GameManager.Instance.OnRestart.AddListener(() => { _loadingPanel.Activate(); });

        GameManager.Instance.OnPlayerFinishEnter.AddListener(() => { _finishPanel.Enable(); });

        GameManager.Instance.OnLoadScene.AddListener(() => { _loadingPanel.Activate(); });
        GameManager.Instance.OnVictory.AddListener(() =>
        {
            _victoryPanel.Enable();
            _finishPanel.Disable();
        });
    }
    private IEnumerator TimerToMethod(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}